using System;

namespace KillEmAll.NET
{
    class Program
    {
        static void pressAnyKeyToExit()
        {
            Console.WriteLine("");
            if (!isRunningAsAdmin())
            {
                // since we're not running as administrator, prompt user to run again as admin...
                Console.Write("Press any key to exit, or [A] to launch as Administrator...");
                ConsoleKeyInfo foo = Console.ReadKey();
                if (foo.KeyChar.ToString().ToLower().Equals("a"))
                {
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
                            Console.WriteLine("You must click [YES] on the following prompt to run as Administrator!  Press a key to try again...");
                            Console.ReadKey();
                            goto StartProcess;
                        }
                    }
                }
            }
            else
            {
                // already running as administrator, prompt to exit only.
                Console.Write("Press any key to exit...");
                Console.ReadKey();
            }
        }

        static bool pressAnyKeyToStart()
        {
            // returns true if 'debug mode' is desired.
            bool bRet = false;
            Console.WriteLine("WARNING:  ANY DATA NOT SAVED WILL BE LOST!  You may close this window to cancel.");
            Console.WriteLine("");
            Console.Write("Press any key to start, or [D] for Debug Mode (prompts before each termination)...");
            ConsoleKeyInfo foo = Console.ReadKey();
            if (foo.KeyChar.ToString().ToLower().Equals("d"))
                bRet = true;
            // always clear console after prompt
            Console.Clear();
            return bRet;
        }

        static bool isRunningAsAdmin() => System.Security.Principal.WindowsIdentity.GetCurrent().Owner.IsWellKnown(System.Security.Principal.WellKnownSidType.BuiltinAdministratorsSid);

        static void Main(string[] args)
        {
            bool bDebugMode = false;
            bool bRunAuto = false;

            // set console title
            var proc = System.Diagnostics.Process.GetCurrentProcess();
            if (isRunningAsAdmin())
                Console.Title = "KillEmAll.NET v" + proc.MainModule.FileVersionInfo.ProductVersion + " (Running as Administrator)";
            else
                Console.Title = "KillEmAll.NET v" + proc.MainModule.FileVersionInfo.ProductVersion + " (Running as User)";

            // check passed command line arguments for the automation/no prompt arg
            foreach (string arg in args)
            {
                string wordOnly = arg.Trim();

                // strip out any / or - passed with the arg
                if (wordOnly.Substring(0, 1).Equals("/"))
                    wordOnly = wordOnly.Substring(1);
                if (wordOnly.Substring(0, 1).Equals("-"))
                    wordOnly = wordOnly.Substring(1);

                // we only expect one arg, so this is sufficient
                if (wordOnly.ToLower().Equals("auto"))
                    bRunAuto = true;
            }

            // if not running automatically, show user prompt...
            if (!bRunAuto)
                bDebugMode = pressAnyKeyToStart();

            // call main functionality here
            try
            {
                KillEmAll foo = new KillEmAll(bDebugMode);
                foo.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Main] Exception: {0}", ex.Message);
            }
            if (!bRunAuto)
                pressAnyKeyToExit();
        }
    }
}
