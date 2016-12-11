using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace ReClassNET.Util
{
	public static class NativeMethods
	{
		#region Constants

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

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct LUID
		{
			public uint LowPart;
			public int HighPart;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct TOKEN_PRIVILEGES
		{
			public uint PrivilegeCount;
			public LUID Luid;
			public uint Attributes;
		}

		#endregion

		#region Natives

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		[DllImport("user32.dll")]
		public static extern int DestroyIcon(IntPtr hIcon);

		[DllImport("dbghelp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static extern int UnDecorateSymbolName(string DecoratedName, StringBuilder UnDecoratedName, int UndecoratedLength, int Flags);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetProcessDPIAware();

		[DllImport("shcore.dll")]
		internal static extern int SetProcessDpiAwareness([MarshalAs(UnmanagedType.U4)] ProcessDpiAwareness a);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool OpenProcessToken(IntPtr ProcessHandle, TokenAccessLevels DesiredAccess, out IntPtr TokenHandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint Zero, IntPtr Null1, IntPtr Null2);

		#endregion

		#region Helper

		public static string UnDecorateSymbolName(string decoratedName)
		{
			Contract.Requires(decoratedName != null);
			Contract.Ensures(Contract.Result<string>() != null);

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
