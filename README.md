
# Time in PC

A Windows application to keep track of time spent in computer.

![Time in PC](https://github.com/al3xandr3/timeinPC/blob/master/timeinPC.png)

[Download Installer](https://github.com/al3xandr3/timeinPC/releases/download/v1/timeinPC-setup.exe "Time in PC Installer")

### What it Does ?

Keeps track and informs about the time spent in computer.

It runs on the Windows System Tray.

When hovering mouse over it, it shows:

- Day: Total time in PC for today, includes inactive periods during the day.
- Active: Time Active in PC for today (excludes inactive periods).

During a working day in computer there might be periods where we stop active use of PC (but are still working), for a meeting, a call, figure something out on paper, etc... So i think is useful to see both numbers: total time and just active time.



### Data Export

To allow re-use of the data collected, it automatically exports on a daily basis into a csv file the data with a summary of the time spent on PC, by day:
- Date: of the day
- Active (minutes): Minutes active in Computer
- Day start time
- Day end time

(Day end time - Day start time)  = gives the total time in PC for the day, the same number as we see in "Day" from the application icon.

Look for timeinPC.csv in the same folder as timeinPC.exe


### Running Requirements

Requires Microsoft Window and the .NET framework to be installed.

## Development Requirements
- F# .NET, open it up in Visual Studio and it should just work.
- Packages:
    + Install-Package System.Data.SQLite.Core
    + Install-Package Costura.Fody

