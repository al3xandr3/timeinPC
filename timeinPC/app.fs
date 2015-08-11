
module app

open System
open System.IO
open System.Threading
open System.Windows.Forms
open System.Drawing

(* Main application:
  1. Keeps the database connection
  2. Manages the thread that checks userInput
  3. Does the UI
*)

[<System.STAThread>]
do let form = new Form()

   let exeFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

   (* Data Export dir*)
   //create folder
   let datafolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Time in PC"
   if System.IO.Directory.Exists(datafolder) = false then
    System.IO.Directory.CreateDirectory(datafolder) |> ignore

   (* 1. Database Open *)
   let connString = sprintf "Data Source=%s;UTF8Encoding=True;Version=3" (exeFolder + @"\timeinPC.sqlite")
   let conn = db.openDB(connString)
   db.setupdb(conn) |> ignore

   // To Export all data to .csv
   let exportDataTotals (conn) =
       let total = db.getTotalMinsByDay(conn)
       let fl = datafolder + @"\timeinPC.csv"
       let wr = new StreamWriter(fl, false)
       wr.WriteLine("Date,Active (minutes),Day Start time,Day End time")
       for day, sum, start, stop in total do
           wr.WriteLine(day + "," + sum.ToString() + "," + start + "," + stop)
       wr.Close()

   // UI: Icon
   let icon = new NotifyIcon()

   // Actions based on time spent in PC
   let doActions(mins) =
       let mins = db.getTodaySum(conn)
       let minsSpan = TimeSpan.FromMinutes((float)mins)
       let minsText = timeSpan.formatTimeSpan minsSpan 2

       let toDay = db.getTodayStartToEnd(conn)
       let minsSpantoDay = TimeSpan.FromMinutes((float)toDay)
       let toDayText = timeSpan.formatTimeSpan minsSpantoDay 2

       // update mouse hover info
       icon.Text <- "Day: " + toDayText + "\nActive: " + minsText
       icon.BalloonTipTitle <- "Time In PC"
       match toDay with
       | 1   -> exportDataTotals(conn) // on 1st min export data
       | 480 -> icon.BalloonTipText <- "8 Hours Now"
                icon.ShowBalloonTip(30000)
       | _ -> () 

   (* 2. user Input check *)
   let checktimeMills = 30000 // half minute in milliseconds (frequency of checks)
   
   // keep checking for the last time user did an input
   let rec inputChecker() = 
     let sinceLastInput, lastActiveInputTime = lastInput.getMsSinceInput()
     if sinceLastInput <= checktimeMills then 
       let success = db.insertMinute(conn, lastActiveInputTime)
       if success <> -1 then doActions() // Do actions if it was successfull
     // call again
     Thread.Sleep(checktimeMills)
     inputChecker()
   
   // Start Input check thread
   let lastInputThread = new Thread(inputChecker)
   lastInputThread.Start()
   
   (* 3. UI Component *)
   let pic = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("timer.ico")
   icon.Icon <- new System.Drawing.Icon(pic)
   icon.Visible <- true
   
   // Menu: Exit
   let cm = new ContextMenu()
   cm.MenuItems.Add("Data", fun _ _ -> 
    exportDataTotals(conn)
    System.Diagnostics.Process.Start(@"explorer.exe", datafolder) |> ignore ) |> ignore
   cm.MenuItems.Add("Exit", fun _ _ -> Environment.Exit(0) ) |> ignore   
   icon.ContextMenu <- cm
   
   Application.Run()
