﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace KillEmAll.NET
{
    class KillEmAll
    {
        private Dictionary<string, string> _internalWindowsFiles = new Dictionary<string, string>();
        private string _winDir;
        private string _sys32;
        private string _sys64;
        private int _myPID;
        private int myParentPID;

        public KillEmAll()
        {
            _winDir = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System)).ToString().ToLower() + "\\";
            _sys32 = _winDir + "system32\\";
            _sys64 = _winDir + "syswow64\\";

            _myPID = Process.GetCurrentProcess().Id;
            myParentPID = ParentProcessUtilities.GetParentProcess(_myPID).Id;
            
            // these are Windows processes that should not be terminated, full paths
            string[] temp = { _winDir + "explorer.exe", _sys32 + "services.exe", _sys32 + "winlogon.exe", _sys32 + "lsass.exe", _sys32 + "logonui.exe", _sys32 + "spoolsv.exe",
                _sys32 + "alg.exe", _sys32 + "lsm.exe", _sys32 + "audiodg.exe", _sys32 + "dllhost.exe", _sys32 + "msdtc.exe", _sys32 + "wscntfy.exe", _sys32 + "wudfhost.exe",
                _sys32 + "wininit.exe", _sys32 + "mdm.exe", _sys32 + "rdpclip.exe", _sys32 + "taskmgr.exe", _sys32 + "dwm.exe", _sys32 + "taskhost.exe", _sys32 + "taskeng.exe",
                _sys32 + "sppsvc.exe", _sys32 + "conhost.exe", _sys32 + "wisptis.exe", _sys32 + "tabtip.exe", _sys32 + "inputpersonalization.exe", _sys32 + "wbem\\wmiprvse.exe",
                _sys64 + "wbem\\wmiprvse.exe", _sys32 + "ui0detect.exe", _sys32 + "sihost.exe", _sys32 + "wlms\\wlms.exe", _sys32 + "smss.exe",
                _sys32 + "csrss.exe", _sys32 + "svchost.exe", _sys64 + "svchost.exe", _sys32 + "dashost.exe", _sys32 + "runtimebroker.exe", _sys32 + "taskhostw.exe",
                _sys32 + "sppsvc.exe", _sys32 + "fontdrvhost.exe", _sys32 + "systemsettingsbroker.exe", _sys32 + "securityhealthservice.exe", _sys32 + "sgrmbroker.exe"};
            foreach (string i in temp)
            {
                try
                {
                    _internalWindowsFiles.Add(i.ToLower(), "");
                }
                catch
                {

                }
            }
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
                    if (PID != 0 && PID != 4 && PID != _myPID && PID != myParentPID)
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
                                    // get the full path AFTER the checks above, because getPathFromPID can throw an exception trying to OpenProcess on PID=0, etc.
                                    string fullpath = getPathFromPID(PID).ToLower();

                                    if (fullpath.Contains("\\"))
                                    {
                                        // we have a full path, so check against full path whitelist
                                        if (!_internalWindowsFiles.ContainsKey(fullpath))
                                            killProcess(fullpath, PID);
                                    }
                                    else
                                    {
                                        // didn't obtain a full path, working with just the filename (this is 99.9% of the time a Windows file in a system dir)
                                        // I didn't bother to create a second/single EXE whitelist, so prepend a system path and check the whitelist...
                                        // don't test for _winDir because we always obtain a path from Explorer.exe, the only file from _winDir in the whitelist.
                                        if (!_internalWindowsFiles.ContainsKey(_sys32 + filename))
                                            if (!_internalWindowsFiles.ContainsKey(_sys64 + filename))
                                                killProcess(filename, PID);
                                    }
                                }
                            }
                        }
                    }
                }
                while (Process32NextW(HANLDE_Processes, ref p32Iw));
            }
        }

        void killProcess(string process, int PID)
        {
            bool bSuccess = false;
            //
            // TODO:  add features for optional termination and to google the process
            //
            //Console.WriteLine($"Terminate process:  \"{process}\"  [Y/n/g] (Yes/No/Google)?");
            //ConsoleKeyInfo foo = Console.ReadKey();
            //if (foo.KeyChar.ToString().ToLower().Equals("y"))
            //{
            //    success = killProcessByPID(PID);
            //    Console.WriteLine($"Terminated={success}  [{process}]");
            //}

            bSuccess = killProcessByPID(PID);
            Console.WriteLine($"Terminated={bSuccess}  [{process}]");
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
            string sRet = "";

            // this call requires Windows Vista+ due to the flag QueryLimitedInformation,
            // see https://docs.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights
            // this call should just fail on XP/2003 so flow continues to use the .NET method below.
            hProcess = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, PID);

            if (hProcess != IntPtr.Zero)
            {
                int size = buffer.Capacity;
                bSuccess = QueryFullProcessImageNameW(hProcess, 0, buffer, ref size);
                sRet = buffer.ToString();
            }
            // actually, the .NET method just throws exceptions on access denied and is not successful retrieving any path that fails above...
            // leaving this code here so I can remember it isn't worth using.
            //
            //if (!bSuccess)
            //{
            //    // on failure above, do it the .NET way, rather than wrestle with GetModuleFileNameExW in C#
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
            return sRet;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
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

        [DllImport("kernel32.dll", EntryPoint = "Process32FirstW", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32FirstW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("kernel32.dll", EntryPoint = "Process32NextW", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32NextW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool QueryFullProcessImageNameW(IntPtr hProcess, uint dwFlags, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpExeName, ref int lpdwSize);

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
            Process process = Process.GetProcessById(id);
            return GetParentProcess(process.Handle);
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
