using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography;

namespace KillEmAll.NET
{
    class KillEmAll
    {
        private string[] _internalPartialFileNameArray;
        private Dictionary<string, string> _internalFileNames = new Dictionary<string, string>();
        private Dictionary<string, string> _internalWindowsFileNames = new Dictionary<string, string>();
        private Dictionary<string, string> _internalWindowsFiles = new Dictionary<string, string>();
        // added to track which processes were already selected to be terminated or skipped in debug mode, to skip prompting the user again
        static Dictionary<string, string> _terminatedProcesses = new Dictionary<string, string>();
        static Dictionary<string, string> _skippedProcesses = new Dictionary<string, string>();
        private string _winDir;
        private string _sys32;
        private string _sys64;
        private int _myPID;
        private int _myParentPID = 0;
        private bool _debugMode;
        private bool _debugModeShowInfo;
        private bool _skipAllAfterTerminatingDebugMode;
        private bool _isWinXP;
        private bool _searchFileNameOnly;
        private string _searchEngineURL;        

        private StringBuilder sbLog = new StringBuilder();
        public string Log() => sbLog.ToString();

        public KillEmAll(bool debugMode = false)
        {
            if (debugMode)
            {
                _debugMode = true;
                // get new settings that are only used in debug mode anyway
                getSettingsFromINI();
            }
            else
                _debugMode = false;

            _internalFileNames.Clear();
            _internalWindowsFileNames.Clear();
            _internalWindowsFiles.Clear();
            _terminatedProcesses.Clear();
            _skippedProcesses.Clear();
            sbLog.Clear();

            // this is reliable even when Environment.OSVersion is lying, because we only care if it is XP/2003 for this variable...
            _isWinXP = Environment.OSVersion.Version.ToString().Substring(0, 1).Equals("5");

            _winDir = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System)).ToString().ToLower() + "\\";
            _sys32 = _winDir + "system32\\";
            _sys64 = _winDir + "syswow64\\";

            // get my process ID for skipping in the Start() loop
            _myPID = Process.GetCurrentProcess().Id;

            // get parent process for the same reason as above...
            // it's ok for _myParentPID to stay at 0 as initialized if parent process isn't running.
            var parentProcess = ParentProcessUtilities.GetParentProcess(_myPID);
            if (parentProcess != null)
                _myParentPID = parentProcess.Id;

            // these are Windows processes that should not be terminated, or that it's pointless to try and terminate, full paths.  
            // of course add 3rd party processes (full paths) as desired, like the last two added for VirtualBox.
            string[] filePathsArr = { _winDir + "explorer.exe", _sys32 + "services.exe", _sys32 + "winlogon.exe", _sys32 + "lsass.exe", _sys32 + "logonui.exe", _sys32 + "spoolsv.exe",
                _sys32 + "alg.exe", _sys32 + "lsm.exe", _sys32 + "audiodg.exe", _sys32 + "dllhost.exe", _sys32 + "msdtc.exe", _sys32 + "wscntfy.exe", _sys32 + "wudfhost.exe",
                _sys32 + "wininit.exe", _sys32 + "mdm.exe", _sys32 + "rdpclip.exe", _sys32 + "taskmgr.exe", _sys32 + "dwm.exe", _sys32 + "taskhost.exe", _sys32 + "taskeng.exe",
                _sys32 + "sppsvc.exe", _sys32 + "conhost.exe", _sys32 + "wisptis.exe", _sys32 + "tabtip.exe", _sys32 + "inputpersonalization.exe", _sys32 + "wbem\\wmiprvse.exe",
                _sys64 + "wbem\\wmiprvse.exe", _sys32 + "ui0detect.exe", _sys32 + "sihost.exe", _sys32 + "ctfmon.exe", _sys32 + "wlms\\wlms.exe", _sys32 + "smss.exe",
                _sys32 + "csrss.exe", _sys32 + "svchost.exe", _sys64 + "svchost.exe", _sys32 + "dashost.exe", _sys32 + "runtimebroker.exe", _sys32 + "taskhostw.exe",
                _sys32 + "sppsvc.exe", _sys32 + "fontdrvhost.exe", _sys32 + "systemsettingsbroker.exe", _sys32 + "securityhealthservice.exe", _sys32 + "sgrmbroker.exe",
                _sys32 + "vboxtray.exe", _sys32 + "vboxservice.exe" };
            foreach (string fullPath in filePathsArr)
            {
                try
                {
                    // add to full path dictionary
                    _internalWindowsFiles.Add(fullPath.ToLower(), "");

                    // now strip path for the filename only dictionary
                    string theFileOnly = StripString(fullPath, "\\", StripStringReturnType.ReturnAfterLastDelimiter);
                    _internalWindowsFileNames.Add(theFileOnly.ToLower(), "");
                }
                catch
                {
                }
            }
            // add any whole filenames to this dictionary - these will be whitelisted regardless of path!
            // the first three are Windows Defender files where the path may not be as predictable and I didn't feel like tracking down everywhere on every OS/version...
            // the second two are for Teamviewer, and they could be in any %programfiles(x86)% or %appdata%...
            // the last is related to our d7x tech tool.
            string[] fileNamesArr = { "msmpeng.exe", "msmpengcp.exe", "nissrv.exe", "tv_w32.exe", "tv_x64.exe", "d7xsvcwait.exe" };
            foreach (string fileName in fileNamesArr)
            {
                try
                {
                    _internalFileNames.Add(fileName.ToLower(), "");
                }
                catch
                {
                }
            }
            // add partial filenames for a 'contains' search - this also ignores path.  this was implemented for remote support software or other 3rd party apps.
            // keep it short and sweet since this is a slow search method, but NOT too generic!  this is whitelisting every file with the exact string
            // included in the filename, regardless of what path it is in!
            _internalPartialFileNameArray = new string[] { "d7x v", "cryptoprevent", "teamviewer", "screenconnect", "lmiguardian", "lmi_", "logmein", 
                                                           "callingcard", "unattended" };
        }

        private void getSettingsFromINI()
        {
            // get new settings that are only used in debug mode anyway
            if (Program.IniRead("Search", "FileNameOnly") == "1")
                _searchFileNameOnly = true;
            else
                _searchFileNameOnly = false;    // doing this else set to FALSE because we might call this to method modify settings not just obtain them to begin with.

            // default to google if not configured
            _searchEngineURL = Program.IniRead("Search", "URL");
            if (_searchEngineURL.Trim().Length < 1)
                _searchEngineURL = "https://www.google.com/search?hl=en&q=";

            // always show info in debug mode
            if (Program.IniRead("DebugMode", "ShowFileInfo") == "1")
                _debugModeShowInfo = true;
            else
                _debugModeShowInfo = false;
        }

        public void Start()
        {
            IntPtr HANLDE_Processes = CreateToolhelp32Snapshot(2, 0);

            PROCESSENTRY32W p32Iw = new PROCESSENTRY32W();
            int size = Marshal.SizeOf(typeof(PROCESSENTRY32W));
            p32Iw.dwSize = Convert.ToUInt32(size);

            bool blFirstProcess = Process32FirstW(HANLDE_Processes, ref p32Iw);
            //int x = Marshal.GetLastWin32Error();
            if (blFirstProcess)
            {
                do
                {
                    // get the process ID for testing
                    int PID = (int)p32Iw.th32ProcessID;

                    // always skip when processID = 0 or 4 which are system critical, or is equal to my processID or my parent process
                    // theory behind skipping the parent process is that it is conhost in a console app, explorer.exe if run by the user
                    // in a GUI based app, or another app in a suite that is using this app as part of it's functionality.
                    if (PID != 0 && PID != 4 && PID != _myPID && PID != _myParentPID)
                    {
                        // check for child processes of this process and skip them, not that KillEmAll.NET does this but other apps I might use this class in might...
                        // so get the parent process ID and make sure that isn't me.
                        int ParentPID = (int)p32Iw.th32ParentProcessID;
                        if (ParentPID != _myPID)
                        {
                            // get the filename
                            string filename = p32Iw.szExeFile.ToLower();

                            // ensure we have a filename string, not sure why this would ever be empty but maybe I ran into it once, I don't recall...
                            if (!string.IsNullOrEmpty(filename.Trim()))
                            {
                                // another quick check for system critical processes (that aren't actually files with extensions)
                                if (!processIsSystemCritical(filename))
                                {
                                    // check the filename only whitelist 
                                    if (!_internalFileNames.ContainsKey(filename))
                                    {
                                        // check an array of partial filenames
                                        bool bPartialMatch = false;
                                        foreach (string partialName in _internalPartialFileNameArray)
                                        {
                                            if (filename.Contains(partialName))
                                            {
                                                bPartialMatch = true;
                                                break;
                                            }
                                        }
                                        // if no match, proceed
                                        if (!bPartialMatch)
                                        {
                                            // get the full path AFTER the checks above, because getPathFromPID can throw an exception trying to OpenProcess on PID=0, etc.
                                            string fullpath = getPathFromPID(PID).ToLower();

                                            if (fullpath.Contains("\\"))
                                            {
                                                // we have a full path, so check against full path whitelist
                                                if (!_internalWindowsFiles.ContainsKey(fullpath))
                                                    killProcess(PID, filename, fullpath);
                                            }
                                            else
                                            {
                                                // on failure to identify a full path, work with the filenames only.
                                                // many Windows files and possibly others will fail path identification when this app is running at user level;
                                                // chances are if we can't identify the path we probably can't terminate the app either, but no harm in trying anyway...
                                                if (!_internalWindowsFileNames.ContainsKey(filename))
                                                    killProcess(PID, filename);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                while (Process32NextW(HANLDE_Processes, ref p32Iw));
            }
        }

        void killProcess(int PID, string processName, string fullPath = "")
        {
            bool bSuccess = false;
            bool bKill = false;
            string sResult = "";

            // use full path (only if we have one) for initial user display and log text, else use process name only.
            string sSubject = processName;
            if (fullPath.Contains("\\"))
                sSubject = fullPath;

            // only if not flagged to skip everything 
            if (!_skipAllAfterTerminatingDebugMode)
            {
                // only if we haven't skipped this process already
                if (!_skippedProcesses.ContainsKey(sSubject))
                {
                    // check to see if we've already terminated this process
                    if (_terminatedProcesses.ContainsKey(sSubject))
                    {
                        // flag to kill and skip the rest
                        bKill = true;
                    }
                    else
                    {
                        // only if in debug mode
                        if (_debugMode)
                        {
                            // use to prevent user from filling the screen with duplicate file info, so we don't have to rewrite the terminate prompt for clarity
                            bool bAlreadyPrintedFileInfo = false;

                        PrintMsg:
                            Console.WriteLine("");
                            Console.Write($"Terminate process:  \"{sSubject}\"  [Y/n] (Yes/No)?");

                            // if configured to always show info
                            if (_debugModeShowInfo)
                            {
                                // even though we test for this in the printFileInfo() method, when tested there it prints an error,
                                // but here when this setting is enabled automatically, it shouldn't print that error, so don't even call the method...
                                if (fullPath.Contains("\\"))
                                {
                                    // flag true to ensure file info is printed only once
                                    bAlreadyPrintedFileInfo = true;
                                    printFileInfo(fullPath, processName);
                                }
                            }

                        GetUserInput:
                            // moved from before GetUserInput: because we may change settings mid-program now and this variable needs to be reinterpreted.
                            // determine search string based on config setting; default to processName when setting does not exist
                            string searchString = "";
                            if (_searchFileNameOnly)
                                searchString = processName;
                            else
                                searchString = sSubject;

                            // read key
                            ConsoleKeyInfo foo = Console.ReadKey();
                            Console.WriteLine("");
                            switch (foo.Key)
                            {
                                case ConsoleKey.Q:
                                    Console.Write("\nTerminate all remaining processes?  [Y/n]  (Yes/No)");
                                    bool terminateAll = false;
                                QuitPrompt:
                                    ConsoleKeyInfo bar = Console.ReadKey();
                                    switch (bar.Key)
                                    {
                                        case ConsoleKey.Enter:
                                            terminateAll = true;
                                            break;
                                        case ConsoleKey.Y:
                                            terminateAll = true;
                                            break;
                                        case ConsoleKey.N:
                                            break;
                                        case ConsoleKey.Escape:
                                            break;
                                        default:
                                            goto QuitPrompt;
                                    }
                                    if (terminateAll)
                                    {
                                        Console.WriteLine("\nQuitting Debug Mode (Terminating all remaining processes). . .\n");
                                        _debugMode = false;
                                        bKill = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nQuitting Debug Mode (Skipping all remaining processes). . .\n");
                                        _debugMode = false;
                                        _skipAllAfterTerminatingDebugMode = true;
                                    }
                                    break;
                                case ConsoleKey.C:
                                    System.Windows.Forms.Form config = new ConfigUI();
                                    config.ShowDialog();
                                    // get potential settings changes from INI
                                    getSettingsFromINI();
                                    goto GetUserInput;
                                case ConsoleKey.G:
                                    webSearch(searchString);
                                    goto GetUserInput;
                                case ConsoleKey.W:
                                    webSearch(searchString);
                                    goto GetUserInput;
                                case ConsoleKey.S:
                                    webSearch(searchString);
                                    goto GetUserInput;
                                case ConsoleKey.I:
                                    if (!bAlreadyPrintedFileInfo)
                                    {
                                        // flag true to ensure file info is printed only once
                                        bAlreadyPrintedFileInfo = true;
                                        printFileInfo(fullPath, processName);
                                    }
                                    goto GetUserInput;
                                case ConsoleKey.O:
                                    openInExplorer(fullPath);
                                    goto GetUserInput;
                                case ConsoleKey.H:
                                    Console.Clear();
                                    Console.WriteLine("");
                                    Program.PrintDebugHelp();
                                    // in case user wants to see that info again
                                    if (!_debugModeShowInfo)
                                        bAlreadyPrintedFileInfo = false;
                                    goto PrintMsg;
                                case ConsoleKey.N:
                                    // add to skipped files collection if user specifically skips this file
                                    if (!_skippedProcesses.ContainsKey(sSubject))
                                        _skippedProcesses.Add(sSubject, "");
                                    break;
                                case ConsoleKey.Escape:
                                    break;
                                case ConsoleKey.Y:
                                    bKill = true;
                                    break;
                                case ConsoleKey.Enter:
                                    bKill = true;
                                    break;
                                default:
                                    goto GetUserInput;
                            }
                        }
                        else
                        {
                            // finally we land here if we aren't in debug mode
                            bKill = true;
                        }
                    }
                }
            }

            if (bKill)
            {
                bSuccess = killProcessByPID(PID);
                if (bSuccess)
                {
                    // add to successfully terminated processes list
                    if (!_terminatedProcesses.ContainsKey(sSubject))
                        _terminatedProcesses.Add(sSubject, "");
                    sResult = "True ";  // pad an extra space to match length of 'FALSE' for text formatting on screen/in log file
                }
                else
                    sResult = "FALSE";  // set text to uppercase to easily recognize a failure
                sbLog.AppendLine($"Terminated={sResult} \"{sSubject}\"");
                Console.WriteLine($"Terminated={sResult} \"{processName}\"");
            }
            else
            {
                Console.WriteLine($"Skipped \"{processName}\"");
            }
        }

        void printFileInfo(string fullPath, string processName)
        {
            // ensure we were passed a path
            if (!fullPath.Contains("\\"))
            {
                Console.WriteLine("\n  [No file path; cannot query file information!]");
                return;
            }

            // first try getting the file date, but don't print it yet.
            string fileDate = "";
            try
            {
                fileDate = File.GetLastWriteTime(fullPath).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException: {0}\n", ex.Message);

                // if we can't get file date, I doubt we're going to get anything else out of the rest of this method so just exit
                return;
            }

            // follow up with file version info strings
            try
            {
                FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(fullPath);

                Console.WriteLine("\n");

                // check the current vs. the original filename
                if (fileInfo.OriginalFilename.ToLower() != processName.ToLower())
                    Console.WriteLine("  Original File = " + fileInfo.OriginalFilename + "  (The file has been renamed from it's compiled value.)");

                if (fileDate.Length > 0)
                    Console.WriteLine("  File Date     = " + fileDate);

                if (fileInfo.FileVersion.Length > 0)
                    Console.WriteLine("  File Version  = " + fileInfo.FileVersion);

                if (fileInfo.ProductName.Length > 0)
                    Console.WriteLine("  Product Name  = " + fileInfo.ProductName);

                if (fileInfo.InternalName.Length > 0)
                    Console.WriteLine("  Internal Name = " + fileInfo.InternalName);

                if (fileInfo.FileDescription.Length > 0)
                    Console.WriteLine("  Description   = " + fileInfo.FileDescription);

                if (fileInfo.Comments.Length > 0)
                    Console.WriteLine("  Comments      = " + fileInfo.Comments);

                if (fileInfo.CompanyName.Length > 0)
                    Console.WriteLine("  Company       = " + fileInfo.CompanyName);

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException: {0}\n", ex.Message);

                // if we can't get this info, again I don't think we're gonna get file hashes, so exit
                return;
            }

            // file hashes
            try
            {
                byte[] myFileData = File.ReadAllBytes(fullPath);
                Console.WriteLine("  MD5 HASH      = " + byteArrayToString(MD5.Create().ComputeHash(myFileData)));
                Console.WriteLine("  SHA256 HASH   = " + byteArrayToString(SHA256.Create().ComputeHash(myFileData)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException: {0}\n", ex.Message);
                return;
            }

            Console.WriteLine("");
        }

        static string byteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        static void openInExplorer(string fullPath)
        {
            if (!fullPath.Contains("\\"))
            {
                Console.WriteLine("\n  [No file path to open!]");
                return;
            }

            try
            {
                Process.Start("explorer.exe", "/select," + fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
        }

        void webSearch(string searchString)
        {
            // pass through the filename filter first, then replace spaces and quotes for URL
            string url = webSearchFileFilter(searchString);
            url = url.Replace(" ", "%20");
            url = url.Replace("\"", "%22");
            url = url.Replace("\\", "%5C");

            try
            {
                Process.Start(_searchEngineURL + url);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    Console.WriteLine($"No web browser detected!  [{noBrowser.Message}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching web browser!  [{ex.Message}]");
            }
        }

        string webSearchFileFilter(string process)
        {
            // strip out variable information from file paths (e.g. drive letter and user directory names)
            string sRet = process.Trim();
            
            // we don't need to do anything here if no full path
            if (sRet.Contains(":"))
            {
                // strip drive letter, colon, and backslash
                sRet = StripString(sRet, "\\", StripStringReturnType.ReturnAfterFirstDelimiter);

                // are we in "Users\"
                if (sRet.Substring(0, 6).ToLower().Contains("users\\"))
                {
                    // strip out "Users\"
                    sRet = sRet.Substring(6);

                    // strip out the next whatever it is 
                    sRet = StripString(sRet, "\\", StripStringReturnType.ReturnAfterFirstDelimiter);

                    // add back in "Users\" and add quotes between Users\" and "sRet but not wrapping quotes around the whole return
                    // (full value will be always wrapped in quotes at the end of this procedure.)
                    sRet = "Users\\\" \"" + sRet;
                }
            }
            else
            {
                // if a UNC path, strip it down to the filename only, since personalized network paths probably don't mean much in web searches...
                if (sRet.StartsWith(@"\\"))
                    sRet = StripString(sRet, "\\", StripStringReturnType.ReturnAfterLastDelimiter);
            }
            // wrap final return in quotes
            return "\"" + sRet + "\"";
        }

        // borrowed with StripString and GetLastPos from my other code, so some of it isn't used here.
        enum StripStringReturnType { ReturnBeforeFirstDelimiter, ReturnAfterFirstDelimiter, ReturnBeforeLastDelimiter, ReturnAfterLastDelimiter }

        string StripString(string OriginalString, string TheDelimiter, StripStringReturnType returnType = StripStringReturnType.ReturnBeforeFirstDelimiter)
        {
            string FirstPart;
            string SecondPart;
            int FirstChr = OriginalString.IndexOf(TheDelimiter);
            if (FirstChr == -1)
            {
                return OriginalString;
            }
            else
            {
                bool lastDelim = false;
                if (returnType == StripStringReturnType.ReturnBeforeLastDelimiter) lastDelim = true;
                if (returnType == StripStringReturnType.ReturnAfterLastDelimiter) lastDelim = true;

                if (lastDelim)
                {
                    int lastPos = 0;
                    lastPos = GetLastPos(OriginalString, TheDelimiter);
                    if (lastPos == -1)
                        return OriginalString;

                    FirstPart = OriginalString.Substring(0, lastPos);
                    SecondPart = OriginalString.Substring(lastPos + TheDelimiter.Length);
                    if (returnType == StripStringReturnType.ReturnBeforeLastDelimiter)
                        return FirstPart;
                    else
                        return SecondPart;
                }
                else
                {
                    FirstPart = OriginalString.Substring(0, FirstChr);
                    SecondPart = OriginalString.Substring(FirstChr + TheDelimiter.Length);
                    if (returnType == StripStringReturnType.ReturnBeforeFirstDelimiter)
                        return FirstPart;
                    else
                        return SecondPart;
                }
            }
        }

        static int GetLastPos(string data, string word, StringComparison StringComp = StringComparison.OrdinalIgnoreCase)
        {
            int start = 0;
            int at = 0;
            int ret = 0;
            int lastPos = -1;
            while (at != -1)
            {
                at = data.IndexOf(word, start, StringComp);
                if (at == -1) break;
                lastPos = at;
                start = at + 1;
                ret++;
            }
            return lastPos;
        }

        bool killProcessByPID(int PID)
        {
            IntPtr hProcess;
            bool bSuccess = false;

            hProcess = OpenProcess(ProcessAccessFlags.Terminate, false, PID);

            if (hProcess != IntPtr.Zero)
                bSuccess = TerminateProcess(hProcess, 0);

            return bSuccess;
        }

        bool processIsSystemCritical(string procName)
        {
            bool bRet = false;
            // if process has no file extension it is a special Windows process
            if (!procName.Contains("."))
            {
                // if string starts with 'system'
                if (procName.Substring(0, 6).ToLower().Equals("system"))
                    bRet = true;
                else if (procName.ToLower().Equals("registry"))
                    bRet = true;
                else if (procName.ToLower().Equals("memory compression"))
                    bRet = true;
            }
            return bRet;
        }

        string getPathFromPID(int PID)
        {
            IntPtr hProcess;
            bool bSuccess = false;
            var buffer = new StringBuilder(1024);
            int size = buffer.Capacity;
            string sRet = "";
            ProcessAccessFlags pFlags;

            // see https://docs.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights
            if (_isWinXP)
                pFlags = ProcessAccessFlags.QueryInformation;
            else
                pFlags = ProcessAccessFlags.QueryLimitedInformation;

            // open process
            hProcess = OpenProcess(pFlags, false, PID);

            if (hProcess != IntPtr.Zero)
            {
                if (!_isWinXP)
                {
                    // this will fail on XP/2003
                    // also, some instances of Vista may not have the right dll version to support this call, hence the try/catch.
                    try
                    {
                        bSuccess = QueryFullProcessImageNameW(hProcess, 0, buffer, ref size);
                    }
                    catch
                    {
                    }
                }
                // actually, the .NET method just throws exceptions on access denied and is not successful retrieving any path that fails above...
                // leaving this code here so I can remember it isn't worth using.
                //
                //if (!bSuccess)
                //{
                //    // on failure above, do it the .NET way
                //    var process = Process.GetProcessById(PID);
                //    // below is throwing exception on access denied
                //    try
                //    {
                //        sRet = process.MainModule.FileName;
                //    }
                //    catch
                //    {
                //    }
                //}
                if (bSuccess)
                    sRet = buffer.ToString();
                else
                {
                    uint ret = GetModuleFileNameExW(hProcess, IntPtr.Zero, buffer, (uint)size);
                    if (ret != 0)
                        sRet = buffer.ToString();
                }
            }
            return sRet;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PROCESSENTRY32W
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szExeFile;
        };
        [DllImport("kernel32.dll", EntryPoint = "CreateToolhelp32Snapshot")]
        public static extern IntPtr CreateToolhelp32Snapshot(UInt32 dwFlags, UInt32 th32ProcessID);

        [DllImport("kernel32.dll", EntryPoint = "Process32FirstW", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32FirstW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("kernel32.dll", EntryPoint = "Process32NextW", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32NextW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool QueryFullProcessImageNameW(IntPtr hProcess, uint dwFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpExeName, ref int lpdwSize);
        
        [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        static extern uint GetModuleFileNameExW(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
    }


    // looking for NtQueryInformationProcess usage, I borrowed this struct from Simon's comment (not the accepted answer)
    // here:  https://stackoverflow.com/questions/394816/how-to-get-parent-process-in-net-in-managed-way
    // which is supposedly the preferred method for speed in C#, but also because I use the same API in d7x code, and I must have had a good reason.
    //
    /// <summary>
    /// A utility class to determine a process parent.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ParentProcessUtilities
    {
        // These members must match PROCESS_BASIC_INFORMATION
        internal IntPtr Reserved1;
        internal IntPtr PebBaseAddress;
        internal IntPtr Reserved2_0;
        internal IntPtr Reserved2_1;
        internal IntPtr UniqueProcessId;
        internal IntPtr InheritedFromUniqueProcessId;

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);

        /// <summary>
        /// Gets the parent process of the current process.
        /// </summary>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess()
        {
            return GetParentProcess(Process.GetCurrentProcess().Handle);
        }

        /// <summary>
        /// Gets the parent process of specified process.
        /// </summary>
        /// <param name="id">The process id.</param>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess(int id)
        {
            try
            {
                Process process = Process.GetProcessById(id);
                return GetParentProcess(process.Handle);
            }
            catch
            {
                // not found
                return null;
            }
        }

        /// <summary>
        /// Gets the parent process of a specified process.
        /// </summary>
        /// <param name="handle">The process handle.</param>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess(IntPtr handle)
        {
            ParentProcessUtilities pbi = new ParentProcessUtilities();
            int returnLength;
            int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
            if (status != 0)
                return null;
            //throw new Win32Exception(status);

            try
            {
                return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
            }
            catch (ArgumentException)
            {
                // not found
                return null;
            }
        }
    }
}
