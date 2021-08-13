
![Time in PC](https://raw.githubusercontent.com/al3xandr3/timeinPC/master/timeinPC.png)

### What it Does ?

Keeps track and informs about the time spent in computer.

It runs on the Windows taskbar and when hovering mouse over it, it shows:
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

### Running it

Requires Microsoft Window and the .NET framework to be installed.

Create a shortcut of the timeinPC.exe into the Startup Applications folder, and let it run on background, no need to input any data or worry about it. 

Startup Applications folder: `C:\Users\<username>\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup`

<script type="text/javascript">
$('aside#sidebar a.button').href = "https://github.com/al3xandr3/timeinPC/releases/download/v1/timeinPC.v1.zip";
</script>