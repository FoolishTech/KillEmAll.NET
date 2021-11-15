using System;
using System.Diagnostics;
using System.IO;

namespace KillEmAll.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            bool bDebugMode = false;
            bool bRunAuto = false;
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

            // set console title with version info
            var proc = Process.GetCurrentProcess();            
            Console.Title = "KillEmAll.NET v" + proc.MainModule.FileVersionInfo.ProductVersion + " (by www.d7xTech.com) - Running as " + userType;

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

            // if not running automatically, show user prompt...
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
            Console.Write(logText);

            // call main functionality here
            try
            {
                KillEmAll foo = new KillEmAll(bDebugMode);
                foo.Start();
                // append to log text
                logText += foo.Log();
                // write end msg
                Console.WriteLine(endMsg);
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
            Console.WriteLine("Press 'D' for Debug Mode (prompt before each program termination)\n");
            Console.Write("Press any other key to start. . .");
            ConsoleKeyInfo foo = Console.ReadKey();
            if (foo.KeyChar.ToString().ToLower().Equals("d"))
                bRet = true;
            // always clear console after prompt
            Console.Clear();
            return bRet;
        }

        static void pressAnyKeyToExit(string logText)
        {
            bool showTextFileOnClose = false;

            Console.WriteLine("");
            if (!isRunningAsAdmin())
            {
                // since we're not running as administrator, prompt user to run again as admin...
                Console.Write("Press 'L' to save results to KillEmAll_Log.txt\nPress 'A' to Run again as Administrator\nPress any other key to exit. . .");
            GetResponse:
                ConsoleKeyInfo foo = Console.ReadKey();
                if (foo.KeyChar.ToString().ToLower().Equals("a"))
                {
                    // if function returns true, flag this bool = false so we don't pop up the text file when we're relaunching as admin
                    if (launchSelfAsAdministrator())
                        showTextFileOnClose = false;
                }
                else if (foo.KeyChar.ToString().ToLower().Equals("l"))
                {
                    logTextToFile(logText);
                    showTextFileOnClose = true;
                    Console.Write("Press 'A' to Run again as Administrator\nPress any other key to exit. . .");
                    goto GetResponse;
                }
            }
            else
            {
                // already running as administrator, prompt to exit only.
                Console.Write("Press 'L' to save results to KillEmAll_Log.txt\nPress any other key to exit. . .");
                ConsoleKeyInfo foo = Console.ReadKey();
                if (foo.KeyChar.ToString().ToLower().Equals("l"))
                {
                    logTextToFile(logText);
                    showTextFileOnClose = true;
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
    }
}
