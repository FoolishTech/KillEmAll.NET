using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace KillEmAll.NET
{
    class Program
    {
        static string _appPath;
        static string _iniFile;
        static string _userType = "Standard User";
        static string _endMsg = "KillEmAll Completed!";
        static bool _bLogToFile = false;
        static bool _showTextFileOnClose = false;
        static string _file_d7xEXE = "";

        static bool _VirusTotalCapable = false;
        public static bool VirusTotalCapable
        {
            get 
            { 
                // once it is set = true, always assume true to avoid checking multiple times
                if (!_VirusTotalCapable)
                {
                    // determine if we're capable of VirusTotal functionality
                    if (File.Exists(_appPath + "\\VirusTotalNet.dll"))
                        if (File.Exists(_appPath + "\\Newtonsoft.Json.dll"))
                            _VirusTotalCapable = true;
                }
                return _VirusTotalCapable;
            }
        }


        static void Main(string[] args)
        {
            bool bDebugMode = false;
            bool bRunAuto = false;
            bool bAutoStart = false;

            // get user type for log text and console title and set end message based on user environment
            if (isRunningAsAdmin())
                _userType = "Administrator";
            else if (isRunningAsSystem())
                _userType = "System";
            else
                _endMsg += "  If you are still experiencing issues, run this app again as Administrator.";

            // create the proc object to set console title with version info and INI file variable
            var proc = Process.GetCurrentProcess();
            Console.Title = "KillEmAll.NET v" + proc.MainModule.FileVersionInfo.ProductVersion + " (by www.d7xTech.com) - Running as " + _userType;
            _appPath = Path.GetDirectoryName(proc.MainModule.FileName);
            // if in root path, strip off the trailing backslash
            if (_appPath.EndsWith("\\"))
                _appPath = _appPath.Substring(0, _appPath.Length - 1);
            _iniFile = _appPath + "\\KillEmAll.NET.ini";

            // before anything else, determine if we need to force launch as admin
            if (!isRunningAsAdmin())
            {
                if (IniRead("Startup", "ForceAdmin") == "1")
                {
                    launchSelfAsAdministrator();
                    Environment.Exit(0);
                }
            }

            // check passed command line arguments for the automation/no prompt arg
            foreach (string arg in args)
            {
                string wordOnly = arg.Trim();

                // strip out any / or - passed with the arg
                if (wordOnly.Substring(0, 1).Equals("/"))
                    wordOnly = wordOnly.Substring(1);
                if (wordOnly.Substring(0, 1).Equals("-"))
                    wordOnly = wordOnly.Substring(1);

                // set flags based on the arg received...
                switch (wordOnly.ToLower())
                {
                    case "auto":
                        bRunAuto = true;    // separate from bAutoStart, this option forces the app to end without prompt as well
                        bAutoStart = true;  // force the app to start without prompt, but not end without prompt
                        break;
                    case "debug":
                        bDebugMode = true;
                        break;
                    case "log":
                        _bLogToFile = true;
                        break;
                }
            }

            // if we didn't set this above with the /auto switch, check the INI settings
            if (!bAutoStart)
                if (IniRead("Startup", "AutoKill") == "1")
                    bAutoStart = true;  // force the app to start without prompt, but not end without prompt

            // if not starting or completely running automatically, show user prompt...
            if (!bAutoStart)
                if (!bDebugMode)
                        bDebugMode = pressAnyKeyToStart();

            // moved the rest to a new method so we can repeat KillEmAll at the end if desired
            startKillEmAll(bDebugMode, bRunAuto);
        }

        static void startKillEmAll(bool bDebugMode = false, bool bRunAuto = false)
        {
            // clear console since this can now be run multiple times in a row
            Console.Clear();

            // determine if we have an active d7x session
            _file_d7xEXE = RegReadValueHKLM("Software\\d7xTech\\d7x\\Session\\Paths", "AppEXE");
            bool bShowd7xOptions = false;
            // set d7x EXE if exist
            if (_file_d7xEXE.Length > 0)
                if (File.Exists(_file_d7xEXE))
                    bShowd7xOptions = true;

            // if we're in debug mode we'll append a string to the end of the logText
            string sDebugAddendum = "";
            if (bDebugMode)
                sDebugAddendum = "  DEBUG MODE";

            // start log text
            DateTime startTime = DateTime.Now;
            string logText = "Started on " + Environment.MachineName + " at " + startTime.ToString("MM/dd/yyyy h:mm:ss tt") + "...  (Running as " + _userType + ")" + sDebugAddendum + "\n\n";

            Console.Write(logText);
            if (bDebugMode)
                PrintDebugHelp(bShowd7xOptions);
            
            // call main functionality here
            try
            {
                KillEmAll foo = new KillEmAll(bDebugMode);
                
                // start terminating processes
                foo.Start();

                // append to log text
                logText += foo.Log();

                // write end msg
                Console.WriteLine("\n" + _endMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Main] Exception: {0}", ex.Message);
            }
            if (!bRunAuto)
                pressAnyKeyToExit(logText); // passing logText so it can be saved if user selects
            else
                if (_bLogToFile) logTextToFile(logText); // log automatically
        }

        static bool pressAnyKeyToStart()
        {
            // returns true if 'debug mode' is desired.
            bool bEnableDebugMode = false;
        PrintMsg:
            Console.WriteLine("WARNING:  ANY DATA NOT SAVED WILL BE LOST!  (Close this window to abort.)\n");
            if (Console.WindowWidth < 120)
            {
                Console.WriteLine("Press 'C' for KillEmAll.NET Configuration");
                Console.WriteLine("Press 'D' to run KillEmAll.NET (in Debug Mode)");
            }
            else
            {
                Console.WriteLine("Press 'D' to run KillEmAll.NET (in Debug Mode)       Press 'C' for KillEmAll.NET Configuration");
            }
            if (!isRunningAsAdmin())
                Console.WriteLine("Press 'A' to run KillEmAll.NET (as Administrator)");
            Console.Write("\nPress any other key to start. . .");
        GetInput:
            ConsoleKeyInfo foo = Console.ReadKey();
            switch (foo.Key)
            {
                case ConsoleKey.A:
                    if (isRunningAsAdmin())
                        goto GetInput;
                    // if function returns true, be sure to end execution of this instance since we're relaunching as admin
                    if (launchSelfAsAdministrator())
                        Environment.Exit(0);
                    break;
                case ConsoleKey.C:
                    System.Windows.Forms.Form config = new ConfigUI();
                    config.ShowDialog();
                    goto GetInput;
                case ConsoleKey.D:
                    bEnableDebugMode = true;
                    break;
                case ConsoleKey.I:
                    printAppInfo();
                    Console.WriteLine("");
                    goto PrintMsg;
                case ConsoleKey.D0:
                    zeroMenu();
                    goto PrintMsg;
            }
            // always clear console after prompt
            Console.Clear();
            return bEnableDebugMode;
        }

        static void pressAnyKeyToExit(string logText)
        {
            string savelogText = "Press 'L' to generate a log file\n";
            string runAsAdminText = "Press 'A' to run KillEmAll.NET again (as Administrator)    \n";
            string pressAnyKeyText = "\nPress any other key to exit. . .";

            // if administrator, empty this string so the option isn't printed
            if (isRunningAsAdmin())
                runAsAdminText = "";

            // construct the rest of the text
            string runAndConfigText;
            if (Console.WindowWidth  < 120)
            {
                runAndConfigText = "Press 'R' to run KillEmAll.NET again\n";
                runAndConfigText += "Press 'D' to run KillEmAll.NET again (in Debug Mode)\n";
                runAndConfigText += "Press 'C' for KillEmAll.NET Configuration\n";
            }
            else
            {
                runAndConfigText = "Press 'R' to run KillEmAll.NET again                      Press 'C' for KillEmAll.NET Configuration\n";
                runAndConfigText += "Press 'D' to run KillEmAll.NET again (in Debug Mode)      \n";
            }

            // if we're already flagged to show log on exit, we've been through this method before and specifically selected 'L' to save the log...
            if (_showTextFileOnClose)
            {
                // so automatically log everything else; pass a silent flag because we don't need to see that every time.
                logTextToFile(logText, true);
                // and clear this string so the 'L' option isn't shown below
                savelogText = "";
            }

        PrintMsg:
            // write a little screen distance
            Console.WriteLine("\n");

            // since we're not running as administrator, prompt user to run again as admin...
            Console.Write(savelogText + runAndConfigText + runAsAdminText + pressAnyKeyText);
        GetResponse:
            ConsoleKeyInfo foo = Console.ReadKey();
            switch (foo.Key)
            {
                case ConsoleKey.R:
                    startKillEmAll();
                    return;
                case ConsoleKey.D:
                    startKillEmAll(true);
                    return;
                case ConsoleKey.A:
                    if (isRunningAsAdmin())
                        goto GetResponse;
                    // if function returns true, flag this bool = false so we don't pop up the text file when we're relaunching as admin
                    if (launchSelfAsAdministrator())
                        _showTextFileOnClose = false;                        
                    break;
                case ConsoleKey.C:
                    System.Windows.Forms.Form config = new ConfigUI();
                    config.ShowDialog();
                    goto GetResponse;
                case ConsoleKey.L:
                    // if we're not flagged to show file on exit, we haven't logged yet, so allow to log text to file
                    if (!_showTextFileOnClose)
                    {
                        logTextToFile(logText);
                        _showTextFileOnClose = true;
                        Console.Write(runAndConfigText + runAsAdminText + pressAnyKeyText);
                    }
                    goto GetResponse;
                case ConsoleKey.I:
                    printAppInfo();
                    goto PrintMsg;
                case ConsoleKey.D0:
                    zeroMenu();
                    goto PrintMsg;
            }

            if (_showTextFileOnClose)
            {
                string logFile = getLogFilePathAndName();
                Process.Start("notepad.exe", logFile);
            }
        }

        static void zeroMenu()
        {
            // just a quick way to delete a file; right now it's just the log file but I can add the allow list and, well I dunno what else...

            string theFile = "";
            string theFileType = "";

            ConsoleKeyInfo foo = Console.ReadKey();
            switch (foo.Key)
            {
                case ConsoleKey.L:
                    theFileType = "Log File";
                    theFile = getLogFilePathAndName();
                    break;
                default:
                    return;
            }

            if (theFile.Length > 0)
            {
                if (!File.Exists(theFile))
                {
                    Console.WriteLine("\n\n{0} does not exist!\n", theFileType);
                    return;
                }

                Console.Write("\n\nDelete {0}?  [y/N] (Yes/No)", theFileType);
            GetInput2:
                ConsoleKeyInfo bar = Console.ReadKey();
                Console.WriteLine("");
                switch (bar.Key)
                {
                    case ConsoleKey.Y:
                        try
                        {
                            File.Delete(theFile);
                            Console.WriteLine("Deleted {0}.", theFileType);
                            // we need to reset flag to show log on exit
                            _showTextFileOnClose = false;
                        }
                        catch
                        {
                            Console.WriteLine("There was an error deleting the file!");
                        }
                        break;
                    case ConsoleKey.N:
                        break;
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.Escape:
                        break;
                    default:
                        goto GetInput2;
                }
            }
            Console.WriteLine("");
        }

        static void printAppInfo()
        {
            string logFile = getLogFilePathAndName();
            string allowListFile = "";
            string d7x3PTPath = "";

            if (_file_d7xEXE.Length < 1)
                _file_d7xEXE = RegReadValueHKLM("Software\\d7xTech\\d7x\\Session\\Paths", "AppEXE");

            if (_file_d7xEXE.Length > 1)
            {
                allowListFile = Path.GetDirectoryName(_file_d7xEXE) + "\\d7x Resources\\Defs\\User\\Process_Whitelist.txt";
                d7x3PTPath = Program.RegReadValueHKLM("Software\\d7xTech\\d7x\\Session\\Paths", "3ptDir");
            }
            else
            {
                // get app path
                var proc = Process.GetCurrentProcess();
                allowListFile = Path.GetDirectoryName(proc.MainModule.FileName) + "\\KillEmAll_Allowed.txt";
            }

            Console.Clear();

            Console.WriteLine("\nVirusTotal  = " + (VirusTotalCapable ? "Both required DLLs found; VirusTotal functionality will be available." : "One or more required DLLs is missing; VirusTotal functionality will be unavailable!"));

            Console.WriteLine("Config File = {0}  (File Exists = {1})", _iniFile, File.Exists(_iniFile).ToString());

            Console.WriteLine("Allow List  = {0}  (File Exists = {1})", allowListFile, File.Exists(allowListFile).ToString());

            Console.WriteLine("Log File    = {0}  (File Exists = {1})", logFile, File.Exists(logFile).ToString());

            if (_file_d7xEXE.Length > 1)
                Console.WriteLine("d7x EXE     = " + _file_d7xEXE);

            if (d7x3PTPath.Length > 1)
                Console.WriteLine("d7x 3PT Dir = " + d7x3PTPath);
        }

        public static void PrintDebugHelp(bool bShowd7xOptions)
        {
            string helpText = "Press 'C' at any time for KillEmAll.NET Configuration.\n";
            helpText += "Press 'H' at any time to display this Help screen.\n";
            helpText += "Press 'Q' at any time to Quit Debug mode.\n\n";

            helpText += "These options are available for every file/process:\n";

            // only print this bit if we aren't automatically showing info
            if (Program.IniRead("DebugMode", "ShowFileInfo") != "1")
                helpText += "Press 'I' at any time for Information on the file.\n";

            helpText += "Press 'O' at any time to Open the file path (if detected) in Explorer.\n";
            helpText += "Press 'P' at any time to Open the file path (if detected) in Command Prompt.\n";
            helpText += "Press 'S' at any time to Search the web.\n";

            // get API key and proceed if exist
            string apiKey = Program.IniRead("VirusTotal", "APIKey");
            if (apiKey.Trim().Length > 1)
                if (VirusTotalCapable)
                    helpText += "Press 'V' at any time to query VirusTotal.\n";

            if (bShowd7xOptions)
            {
                helpText += "\nAn active d7x Session was found!  Additional options are available:\n";
                helpText += "Press 'E' at any time to Examine the file with d7x.\n";
                helpText += "Press 'R' at any time to start a Registry search with d7x.\n";
            }

            Console.WriteLine(helpText);
        }

        static bool launchSelfAsAdministrator()
        {
            bool ret = false;   // return true if we successfully launch as admin.
            const int ERROR_CANCELLED = 1223;
            var myProc = Process.GetCurrentProcess();
            var p = new Process();
            p.StartInfo.FileName = myProc.MainModule.FileName;
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.Verb = "runas";
        StartProcess:
            try
            {
                p.Start();
                ret = true;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_CANCELLED)
                {
                    // user cancelled the UAC prompt?!  give 'em another chance...
                    Console.Clear();
                    Console.WriteLine("\nYou must click 'YES' on the following prompt to run as Administrator\n\nPress any key to try again, or close this window to exit. . .");
                    Console.ReadKey();
                    goto StartProcess;
                }
            }
            return ret;
        }

        static void logTextToFile(string logText, bool bSilent = false)
        {
            string logFile = getLogFilePathAndName();
            try
            {
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine(logText);

                    if (File.Exists(logFile))
                        if (!bSilent)
                            Console.WriteLine("\n\n--> Log Saved!\n");
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        static string getLogFilePathAndName()
        {
            string logPath = RegReadValueHKLM("Software\\d7xTech\\d7x\\Session\\Paths", "ReportsDir");
            if ((logPath.Trim().Length < 1) || (!Directory.Exists(logPath)))
            {
                var proc = Process.GetCurrentProcess();
                logPath = Path.GetDirectoryName(proc.MainModule.FileName);
            }
            return logPath + "\\KillEmAll_Log.txt";
        }

        static bool isRunningAsAdmin() => System.Security.Principal.WindowsIdentity.GetCurrent().Owner.IsWellKnown(System.Security.Principal.WellKnownSidType.BuiltinAdministratorsSid);

        static bool isRunningAsSystem()
        {
            bool ret = false;

            // are we running as system - first check for an empty %username% environment variable...
            string username = Environment.ExpandEnvironmentVariables("%username%");
            if (string.IsNullOrEmpty(username.Trim()))
                ret = true;
            else
            {
                // next check to see if %username% == %computername%$ <- with a dollar sign after it (e.g. "USER" == "USER$")
                string computername = Environment.ExpandEnvironmentVariables("%computername%");
                if (username.ToLower().Equals(computername.ToLower() + "$"))
                    ret = true;
            }
            return ret;
        }


        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string defaultVal, StringBuilder retVal, int size, string lpFileName);

        public static string IniRead(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, _iniFile);
            return temp.ToString();
        }

        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string lpFileName);

        public static bool IniWrite(string Section, string Key, string Value)
        {
            return WritePrivateProfileString(Section, Key, Value, _iniFile);
        }


        public static string RegReadValueHKLM(string regPath, string valueName)
        {
            RegistryKey regKey = null;
            string ret = "";
            RegistryHive theHive = RegistryHive.LocalMachine;
            RegistryView theView = RegistryView.Registry64;

            try
            {
                regKey = RegistryKey.OpenBaseKey(theHive, theView);
            }
            catch
            {
                return ret;
            }

            try
            {
                regKey = regKey.OpenSubKey(regPath);
            }
            catch
            {
                return ret;
            }

            if (regKey != null)
            {
                try
                {
                    ret = regKey.GetValue(valueName).ToString();
                    regKey.Close();
                }
                catch
                {
                    regKey.Close();
                }
            }
            return ret;
        }

        public static string FileToString(string file)
        {
            string contents = "";

            if (!File.Exists(file))
                return contents;

            try
            {
                contents = File.ReadAllText(file);
            }
            catch
            {
            }
            return contents;
        }

    }
}
