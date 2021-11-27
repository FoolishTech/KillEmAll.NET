using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace KillEmAll.NET
{
    class Program
    {
        static string iniFile;

        static void Main(string[] args)
        {
            bool bDebugMode = false;
            bool bRunAuto = false;
            bool bAutoStart = false;
            bool bLogToFile = false;

            // get user type for log text and console title and set end message based on user environment
            string userType = "Standard User";
            string endMsg = "KillEmAll Completed!";
            if (isRunningAsAdmin())
                userType = "Administrator";
            else if (isRunningAsSystem())
                userType = "System";
            else
                endMsg += "  If you are still experiencing issues, run this app again as Administrator.";

            // create the proc object to set console title with version info and INI file variable
            var proc = Process.GetCurrentProcess();
            Console.Title = "KillEmAll.NET v" + proc.MainModule.FileVersionInfo.ProductVersion + " (by www.d7xTech.com) - Running as " + userType;
            iniFile = Path.GetDirectoryName(proc.MainModule.FileName) + "\\KillEmAll.NET.ini";

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
                        bRunAuto = true;
                        break;
                    case "debug":
                        bDebugMode = true;
                        break;
                    case "log":
                        bLogToFile = true;
                        break;
                }
            }

            if (IniRead("Startup", "AutoKill") == "1")
                bAutoStart = true;

            // if not running automatically, show user prompt...
            if (!bAutoStart)
                if (!bDebugMode)
                    if (!bRunAuto)
                        bDebugMode = pressAnyKeyToStart();

            // if we're in debug mode we'll append a string to the end of the logText
            string sDebugAddendum = "";
            if (bDebugMode)
                sDebugAddendum = "  DEBUG MODE";

            // start log text
            DateTime startTime = DateTime.Now;
            string logText = "Started on " + Environment.MachineName + " at " + startTime.ToString("MM/dd/yyyy h:mm:ss tt") + "...  (Running as " + userType + ")" + sDebugAddendum + "\n\n";

            if (bDebugMode)
                Console.Write(logText + "Press 'A' at any time to Abort Debug mode and terminate all processes.\n");
            else
                Console.Write(logText);

            // call main functionality here
            try
            {
                KillEmAll foo = new KillEmAll(bDebugMode);
                foo.Start();
                // append to log text
                logText += foo.Log();
                // write end msg
                Console.WriteLine("\n" + endMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Main] Exception: {0}", ex.Message);
            }
            if (!bRunAuto)
                pressAnyKeyToExit(logText); // passing logText so it can be saved if user selects
            else
                if (bLogToFile) logTextToFile(logText); // log automatically
        }

        static bool pressAnyKeyToStart()
        {
            // returns true if 'debug mode' is desired.
            bool bRet = false;
            Console.WriteLine("WARNING:  ANY DATA NOT SAVED WILL BE LOST!  (Close this window to abort.)\n");
            Console.WriteLine("Press 'C' for KillEmAll.NET Configuration");
            Console.WriteLine("Press 'D' for Debug Mode (prompt before each program termination)");
            Console.Write("Press any other key to start. . .");
        GetInput:
            ConsoleKeyInfo foo = Console.ReadKey();
            if (foo.KeyChar.ToString().ToLower().Equals("c"))
            {
                System.Windows.Forms.Form config = new ConfigUI();
                config.ShowDialog();
                goto GetInput;
            }
            if (foo.KeyChar.ToString().ToLower().Equals("d"))
                bRet = true;
            // always clear console after prompt
            Console.Clear();
            return bRet;
        }

        static void pressAnyKeyToExit(string logText)
        {
            bool showTextFileOnClose = false;

            // if we're starting up and killing automatically without prompt, we need to know we can get to Config at the end...
            string appendText = "";
            if (IniRead("Startup", "AutoKill") == "1")
                appendText = "Press 'C' for KillEmAll.NET Configuration\n";

            Console.WriteLine("");
            if (!isRunningAsAdmin())
            {
                // since we're not running as administrator, prompt user to run again as admin...
                Console.Write("Press 'L' to save results to KillEmAll_Log.txt\n" + appendText + "Press 'A' to Run again as Administrator\nPress any other key to exit. . .");
            GetResponse:
                ConsoleKeyInfo foo = Console.ReadKey();
                string key = foo.KeyChar.ToString().ToLower();
                switch (key)
                {
                    case "a":
                        // if function returns true, flag this bool = false so we don't pop up the text file when we're relaunching as admin
                        if (launchSelfAsAdministrator())
                            showTextFileOnClose = false;
                        break;
                    
                    case "c":
                        System.Windows.Forms.Form config = new ConfigUI();
                        config.ShowDialog();
                        goto GetResponse;

                    case "l":
                        logTextToFile(logText);
                        showTextFileOnClose = true;
                        Console.Write(appendText + "Press 'A' to Run again as Administrator\nPress any other key to exit. . .");
                        goto GetResponse;
                }
            }
            else
            {
                // already running as administrator, prompt to exit only.
                Console.Write("Press 'L' to save results to KillEmAll_Log.txt\n" + appendText + "Press any other key to exit. . .");
            GetResponse2:
                ConsoleKeyInfo foo = Console.ReadKey();
                string key = foo.KeyChar.ToString().ToLower();
                switch (key)
                {
                    case "c":
                        System.Windows.Forms.Form config = new ConfigUI();
                        config.ShowDialog();
                        goto GetResponse2;

                    case "l":
                        logTextToFile(logText);
                        showTextFileOnClose = true;
                        break;
                }
            }

            if (showTextFileOnClose)
            {
                string logFile = getLogFilePathAndName();
                Process.Start("notepad.exe", logFile);
            }
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

        static void logTextToFile(string logText)
        {
            string logFile = getLogFilePathAndName();
            try
            {
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine(logText);

                    if (File.Exists(logFile))
                        Console.WriteLine("\n--> Log Saved!\n");
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        static string getLogFilePathAndName()
        {
            var proc = Process.GetCurrentProcess();
            string logFile = Path.GetDirectoryName(proc.MainModule.FileName) + "\\KillEmAll_Log.txt";
            return logFile;
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
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniFile);
            return temp.ToString();
        }

        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string lpFileName);

        public static bool IniWrite(string Section, string Key, string Value)
        {
            return WritePrivateProfileString(Section, Key, Value, iniFile);
        }

    }
}
