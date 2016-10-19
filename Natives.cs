using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ReClassNET
{
	public static class Natives
	{
		#region Constants

		public const int PROCESS_TERMINATE = 0x0001;
		public const int PROCESS_CREATE_THREAD = 0x0002;
		public const int PROCESS_SET_SESSIONID = 0x0004;
		public const int PROCESS_VM_OPERATION = 0x0008;
		public const int PROCESS_VM_READ = 0x0010;
		public const int PROCESS_VM_WRITE = 0x0020;
		public const int PROCESS_DUP_HANDLE = 0x0040;
		public const int PROCESS_CREATE_PROCESS = 0x0080;
		public const int PROCESS_SET_QUOTA = 0x0100;
		public const int PROCESS_SET_INFORMATION = 0x0200;
		public const int PROCESS_QUERY_INFORMATION = 0x0400;
		public const int PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
		public const int STANDARD_RIGHTS_REQUIRED = 0x000F0000;
		public const int SYNCHRONIZE = 0x00100000;
		public const int PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;

		[Flags]
		public enum AllocationProtectEnum : uint
		{
			PAGE_EXECUTE = 0x00000010,
			PAGE_EXECUTE_READ = 0x00000020,
			PAGE_EXECUTE_READWRITE = 0x00000040,
			PAGE_EXECUTE_WRITECOPY = 0x00000080,
			PAGE_NOACCESS = 0x00000001,
			PAGE_READONLY = 0x00000002,
			PAGE_READWRITE = 0x00000004,
			PAGE_WRITECOPY = 0x00000008,
			PAGE_GUARD = 0x00000100,
			PAGE_NOCACHE = 0x00000200,
			PAGE_WRITECOMBINE = 0x00000400
		}

		public enum StateEnum : uint
		{
			MEM_COMMIT = 0x1000,
			MEM_FREE = 0x10000,
			MEM_RESERVE = 0x2000
		}

		public enum TypeEnum : uint
		{
			MEM_IMAGE = 0x1000000,
			MEM_MAPPED = 0x40000,
			MEM_PRIVATE = 0x20000
		}

		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0;
		public const uint SHGFI_SMALLICON = 0x1;

		internal enum ProcessDpiAwareness : uint
		{
			Unaware = 0,
			SystemAware = 1,
			PerMonitorAware = 2
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		public struct SHFILEINFO
		{
			public IntPtr hIcon;
			public IntPtr iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		#endregion

		#region Natives

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		[DllImport("User32.dll")]
		public static extern int DestroyIcon(IntPtr hIcon);

		[DllImport("dbghelp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static extern int UnDecorateSymbolName(string DecoratedName, StringBuilder UnDecoratedName, int UndecoratedLength, int Flags);

		[DllImport("User32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetProcessDPIAware();

		[DllImport("ShCore.dll")]
		internal static extern int SetProcessDpiAwareness([MarshalAs(UnmanagedType.U4)] ProcessDpiAwareness a);

		#endregion

		#region Helper

		public static string UnDecorateSymbolName(string decoratedName)
		{
			var sb = new StringBuilder(255);
			if (UnDecorateSymbolName(decoratedName, sb, sb.Capacity, /*UNDNAME_NAME_ONLY*/0x1000) != 0)
			{
				return sb.ToString();
			}
			return decoratedName;
		}

		#endregion
	}
}
