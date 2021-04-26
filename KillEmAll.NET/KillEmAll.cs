using System;
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
        private string winDir;
        private string sys32;
        private string sys64;
        private int myPID;
        private int myParentPID;

        public KillEmAll()
        {
            winDir = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System)).ToString().ToLower() + "\\";
            sys32 = winDir + "system32\\";
            sys64 = winDir + "syswow64\\";

            myPID = Process.GetCurrentProcess().Id;
            myParentPID = ParentProcessUtilities.GetParentProcess(myPID).Id;

            string[] temp = { winDir + "explorer.exe", sys32 + "services.exe", sys32 + "winlogon.exe", sys32 + "lsass.exe", sys32 + "logonui.exe", sys32 + "spoolsv.exe",
                sys32 + "alg.exe", sys32 + "lsm.exe", sys32 + "audiodg.exe", sys32 + "dllhost.exe", sys32 + "msdtc.exe", sys32 + "wscntfy.exe", sys32 + "wudfhost.exe",
                sys32 + "wininit.exe", sys32 + "mdm.exe", sys32 + "rdpclip.exe", sys32 + "taskmgr.exe", sys32 + "dwm.exe", sys32 + "taskhost.exe", sys32 + "taskeng.exe",
                sys32 + "sppsvc.exe", sys32 + "conhost.exe", sys32 + "wisptis.exe", sys32 + "tabtip.exe", sys32 + "inputpersonalization.exe", sys32 + "wbem\\wmiprvse.exe",
                sys64 + "wbem\\wmiprvse.exe", sys32 + "ui0detect.exe", sys32 + "cacls.exe", sys32 + "sihost.exe", sys32 + "wlms\\wlms.exe", sys32 + "smss.exe",
                sys32 + "csrss.exe", sys32 + "svchost.exe", sys64 + "svchost.exe", sys32 + "dashost.exe", sys32 + "runtimebroker.exe", sys32 + "taskhostw.exe",
                sys32 + "sppsvc.exe", sys32 + "fontdrvhost.exe", sys32 + "systemsettingsbroker.exe", sys32 + "securityhealthservice.exe", sys32 + "sgrmbroker.exe"};
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

        public void doWork()
        {
            IntPtr HANLDE_Processes = CreateToolhelp32SnapshotRtlMoveMemory(2, 0);

            PROCESSENTRY32W p32Iw = new PROCESSENTRY32W();
            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(PROCESSENTRY32W));
            p32Iw.dwSize = Convert.ToUInt32(size);
            bool blFirstProcess = Process32First(HANLDE_Processes, ref p32Iw);
            int x = Marshal.GetLastWin32Error();

            if (blFirstProcess)
            {
                do
                {
                    int PID = (int)p32Iw.th32ProcessID;
                    string filename = p32Iw.szExeFile.ToLower();
                    string fullpath = getPathFromPID(PID).ToLower();

                    if (PID != 0 && PID != 4 && PID != myPID && PID != myParentPID)
                    {
                        if (!string.IsNullOrEmpty(filename.Trim()))
                        {
                            if (!processIsSystemCritical(filename))
                            {
                                int ParentPID = (int)p32Iw.th32ParentProcessID;
                                if (ParentPID != myPID)
                                {
                                    if (fullpath.Contains("\\"))
                                    {
                                        // check against full path whitelist
                                        if (!_internalWindowsFiles.ContainsKey(fullpath))
                                            killProcess(fullpath, PID);
                                    }
                                    else
                                    {
                                        // check against single file whitelist; but since I don't have one...
                                        if (!_internalWindowsFiles.ContainsKey(winDir + filename))
                                            if (!_internalWindowsFiles.ContainsKey(sys32 + filename))
                                                if (!_internalWindowsFiles.ContainsKey(sys64 + filename))
                                                    killProcess(filename, PID);
                                    }
                                }
                            }
                        }
                    }
                }
                while (Process32Next(HANLDE_Processes, ref p32Iw));
            }
        }

        void killProcess(string process, int PID)
        {
            bool success;
            //Console.WriteLine($"Terminate process:  \"{process}\"  [Y/n/g] (Yes/No/Google)?");
            //ConsoleKeyInfo foo = Console.ReadKey();
            //if (foo.KeyChar.ToString().ToLower().Equals("y"))
            //{
            //    //Console.WriteLine($"Terminating {process}...");
            //    success = killProcessByPID(PID);
            //    Console.WriteLine($"Terminate = {success} for {process}");
            //}
            //if (foo.KeyChar.ToString().ToLower().Equals("g"))
            //    Console.WriteLine($"Searching Web {process}...");

            success = killProcessByPID(PID);
            Console.WriteLine($"Terminate = {success} for {process}");
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
            if (!procName.Contains("."))
            {
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
            bool bSuccess;
            var buffer = new StringBuilder(1024);

            hProcess = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, PID);

            if (hProcess != IntPtr.Zero)
            {
                int size = buffer.Capacity;
                bSuccess = QueryFullProcessImageName(hProcess, 0, buffer, ref size);

                return buffer.ToString();
            }
            else
            {
                return $"Failure!  PID={PID}";
            }
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
        public static extern IntPtr CreateToolhelp32SnapshotRtlMoveMemory(UInt32 dwFlagsdes, UInt32 th32ProcessID);

        [DllImport("kernel32", EntryPoint = "Process32First", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("kernel32", EntryPoint = "Process32Next", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

        [DllImport("kernel32", EntryPoint = "CloseHandle")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool QueryFullProcessImageName(IntPtr hProcess, uint dwFlags, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpExeName, ref int lpdwSize);
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
