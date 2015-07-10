
module db

(* setup to interact with the sqlite db *)

open System
open System.Data
open System.Data.SQLite

// Use always the same open connection, to avoid locks
let openDB (connString:string) = 
  // get connection open
  let cn = new SQLiteConnection(connString)
  cn.Open()
  cn

let executeNonQuery(conn, sql) =
  try
    let cmd = new SQLiteCommand(sql, conn)
    cmd.ExecuteNonQuery()
  with error -> 
    printfn "%s" error.Message
    -1 // telling that it went bad

let executeQuery (conn, sql) =
  let cmd = new SQLiteCommand(sql, conn)
  cmd.ExecuteReader()

let getFirstRow(conn, sql) =
  let reader = executeQuery(conn, sql)
  reader.Read() |> ignore
  reader

///////////

// create table if not existing
let setupdb (conn) = 
  executeNonQuery(conn, "create table if not exists active (minute text primary key not null, day text NOT NULL)")

// insert a active minute
let insertMinute(conn, minute:DateTime) =
  executeNonQuery(conn, 
      "insert into active (minute, day) values ('" + minute.ToString("yyyy-MM-dd HH:mm") + "', '" + minute.ToString("yyyy-MM-dd")+ "')" )

let getTodaySum(conn) =
  let reader = getFirstRow (conn, "select count(*) from active where day ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'")
  reader.GetInt32(0)

let getTodayStartToEnd(conn) =
  let today = DateTime.Now.ToString("yyyy-MM-dd")
  let sql = 
     "select  (strftime('%s', max(minute)) - (select strftime('%s', min(minute)) from active where day ='" + today + "')) / 60 from active where day ='" + today + "'"
  let reader = getFirstRow (conn, sql)
  reader.GetInt32(0)

let getTotalMinsByDay(conn) =
  let reader = executeQuery(conn, "select day, count(*), min(minute), max(minute) from active  where day < '" + DateTime.Now.ToString("yyyy-MM-dd") + "' group by day")
  [while reader.Read() do yield (reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3))]


