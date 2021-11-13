using System;
using System.IO;

namespace KillEmAll.NET
{
    class Program
    {
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
                    // so we don't pop up the text file when we're relaunching as admin
                    showTextFileOnClose = false;

                    const int ERROR_CANCELLED = 1223;
                    var myProc = System.Diagnostics.Process.GetCurrentProcess();
                    var p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = myProc.MainModule.FileName;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                StartProcess:
                    try
                    {
                        p.Start();
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        if (ex.NativeErrorCode == ERROR_CANCELLED)
                        {
                            // user cancelled the UAC prompt?!  give 'em another chance...
                            Console.Clear();
                            Console.WriteLine("You must click 'YES' on the following prompt to run as Administrator!  Press a key to try again...");
                            Console.ReadKey();
                            goto StartProcess;
                        }
                    }
                }
                else if (foo.KeyChar.ToString().ToLower().Equals("l"))
                {
                    LogTextToFile(logText);
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
                    LogTextToFile(logText);
                    showTextFileOnClose = true;
                }
            }

            if (showTextFileOnClose)
            {
                var proc = System.Diagnostics.Process.GetCurrentProcess();
                string logFile = Path.GetDirectoryName(proc.MainModule.FileName) + "\\KillEmAll_Log.txt";
                System.Diagnostics.Process.Start("notepad.exe", logFile);
            }
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

        static void LogTextToFile(string logText)
        {
            var proc = System.Diagnostics.Process.GetCurrentProcess();
            string logFile = Path.GetDirectoryName(proc.MainModule.FileName) + "\\KillEmAll_Log.txt";
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

        static bool isRunningAsAdmin() => System.Security.Principal.WindowsIdentity.GetCurrent().Owner.IsWellKnown(System.Security.Principal.WellKnownSidType.BuiltinAdministratorsSid);

        static void Main(string[] args)
        {
            bool bDebugMode = false;
            bool bRunAuto = false;
            bool bLogToFile = false;

            // get user type for log text and console title
            string userType = "Standard User";
            if (isRunningAsAdmin()) userType = "Administrator";

            // set console title
            var proc = System.Diagnostics.Process.GetCurrentProcess();
            Console.Title = "KillEmAll.NET v" + proc.MainModule.FileVersionInfo.ProductVersion + " (by www.d7xTech.com)  Running as " + userType;

            // check passed command line arguments for the automation/no prompt arg
            foreach (string arg in args)
            {
                string wordOnly = arg.Trim();

                // strip out any / or - passed with the arg
                if (wordOnly.Substring(0, 1).Equals("/"))
                    wordOnly = wordOnly.Substring(1);
                if (wordOnly.Substring(0, 1).Equals("-"))
                    wordOnly = wordOnly.Substring(1);

                // we only expect one arg, so this is sufficient -- ok this is sloppy but I added another arg...
                if (wordOnly.ToLower().Equals("auto"))
                    bRunAuto = true;
                else if (wordOnly.ToLower().Equals("log"))
                    bLogToFile = true;
            }

            // if not running automatically, show user prompt...
            if (!bRunAuto)
                bDebugMode = pressAnyKeyToStart();

            // start log text
            DateTime startTime = DateTime.Now;
            string logText = "Started on " + Environment.MachineName + " at " + startTime.ToString("MM/dd/yyyy h:mm:ss tt") + "...  (Running as " + userType + ")\n\n";
            Console.Write(logText);

            // call main functionality here
            try
            {
                KillEmAll foo = new KillEmAll(bDebugMode);
                foo.Start();
                logText += foo.Log();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Main] Exception: {0}", ex.Message);
            }
            if (!bRunAuto)
                pressAnyKeyToExit(logText);
            else
                if (bLogToFile) LogTextToFile(logText);
        }
    }
}
