# KillEmAll.NET (VirusTotal Edition)
C# implementation of KillEmAll from https://www.d7xtech.com/killemall/
  
KillEmAll.NET is a ‘panic‘ button with an ‘ask questions later‘ attitude, designed to terminate unnecessary programs (those non-essential to Windows) and report the results in a console window.  
  
WARNING:  Any data not saved will be lost!  
  
KillEmAll.NET.exe does not run as Administrator by default, so it will fail to terminate any programs running as Administrator unless you run KillEmAll.NET.exe as Administrator.  
  
You may script KillEmAll.NET automatically using the '/auto' switch, e.g. "KillEmAll.NET.exe /auto" 

Additionally you can add '/log' to '/auto' in order to write the results to the KillEmAll_Log.txt file, e.g. "KillEmAll.NET.exe /auto /log"
  
Requires .NET Framework 4.5  (Note the non-VirusTotal Edition in the master branch only requires .NET Framework 4.0)

VirusTotal functionality requires:
- VirusTotalNet.dll (change project to target .NET Framework 4.5) and it's required Newtonsoft.Json.dll in the same directory as KillEmAll.NET.exe; that project source is available here:  https://github.com/Genbox/VirusTotalNet and it is also available under the MIT license.
- A VirusTotal public API key, which you can get by creating an account in the VirusTotal Community https://www.virustotal.com/gui/join-us  Once registered and signed in, the API key is available through the dropdown menu under your username.
