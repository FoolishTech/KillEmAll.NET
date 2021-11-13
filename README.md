# KillEmAll.NET
C# implementation of KillEmAll from https://www.d7xtech.com/killemall/
  
KillEmAll.NET is a ‘panic‘ button with an ‘ask questions later‘ attitude, designed to terminate unnecessary programs (those non-essential to Windows) and report the results in a console window.  
  
WARNING:  Any data not saved will be lost!  
  
KillEmAll.NET.exe does not run as Administrator by default, so it will fail to terminate any programs running as Administrator unless you run KillEmAll.NET.exe as Administrator.  
  
You may script KillEmAll.NET automatically using the '/auto' switch, e.g. "KillEmAll.NET.exe /auto" 

Additionally you can add '/log' to '/auto' in order to write the results to the KillEmAll_Log.txt file, e.g. "KillEmAll.NET.exe /auto /log"
  
Requires .NET Framework 4.0
