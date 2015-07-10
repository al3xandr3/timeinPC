
module lastInput

(* Reaches using interop to a windows OS native function *)

open System
open System.Runtime.InteropServices 
open Microsoft.FSharp.NativeInterop

[<Struct>]
type LASTINPUTINFO = 
  val mutable cbSize : uint32
  val dwTime : uint32

[<DllImport("user32.dll")>]
extern bool GetLastInputInfo(IntPtr p)

let getMsSinceInput() = 
  let mutable time = LASTINPUTINFO(cbSize = 8u)
  GetLastInputInfo(NativePtr.toNativeInt &&time) |> ignore
  let milliSince = (System.Environment.TickCount - (int)time.dwTime)
  let lastActiveInputTime = DateTime.Now.AddMilliseconds(-(float)milliSince)
  (milliSince, lastActiveInputTime)
