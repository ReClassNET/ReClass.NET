using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET.Native
{
	internal class NativeMethodsWindows : INativeMethods
	{
		#region Imports

		[DllImport("kernel32.dll", ExactSpelling = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern bool FreeLibrary(IntPtr hModule);

		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0;
		public const uint SHGFI_SMALLICON = 0x1;

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

		[DllImport("shell32.dll")]
		private static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbSizeFileInfo, uint uFlags);

		[DllImport("user32.dll", ExactSpelling = true)]
		private static extern int DestroyIcon(IntPtr hIcon);

		[DllImport("advapi32.dll", ExactSpelling = true)]
		private static extern bool OpenProcessToken(IntPtr ProcessHandle, TokenAccessLevels DesiredAccess, out IntPtr TokenHandle);

		[DllImport("advapi32.dll", ExactSpelling = true)]
		private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint Zero, IntPtr Null1, IntPtr Null2);

		[DllImport("dbghelp.dll", CharSet = CharSet.Unicode)]
		private static extern int UnDecorateSymbolName(string DecoratedName, StringBuilder UnDecoratedName, int UndecoratedLength, int Flags);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetProcessDPIAware();

		private enum ProcessDpiAwareness : uint
		{
			Unaware = 0,
			SystemAware = 1,
			PerMonitorAware = 2
		}

		[DllImport("shcore.dll")]
		private static extern int SetProcessDpiAwareness([MarshalAs(UnmanagedType.U4)] ProcessDpiAwareness a);

		#endregion

		IntPtr INativeMethods.LoadLibrary(string fileName)
		{
			return LoadLibrary(fileName);
		}

		IntPtr INativeMethods.GetProcAddress(IntPtr handle, string name)
		{
			return GetProcAddress(handle, name);
		}

		void INativeMethods.FreeLibrary(IntPtr handle)
		{
			FreeLibrary(handle);
		}

		public Icon GetIconForFile(string path)
		{
			var shinfo = new SHFILEINFO();
			if (!SHGetFileInfo(path, 0, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON).IsNull())
			{
				var icon = Icon.FromHandle(shinfo.hIcon).Clone() as Icon;
				DestroyIcon(shinfo.hIcon);
				return icon;
			}

			return null;
		}

		public void EnableDebugPrivileges()
		{
			if (OpenProcessToken(System.Diagnostics.Process.GetCurrentProcess().Handle, TokenAccessLevels.AllAccess, out var token))
			{
				var privileges = new TOKEN_PRIVILEGES
				{
					PrivilegeCount = 1,
					Luid =
					{
						LowPart = 0x14,
						HighPart = 0
					},
					Attributes = 2
				};

				AdjustTokenPrivileges(token, false, ref privileges, 0, IntPtr.Zero, IntPtr.Zero);

				CloseHandle(token);
			}
		}

		public string UndecorateSymbolName(string name)
		{
			var sb = new StringBuilder(255);
			if (UnDecorateSymbolName(name, sb, sb.Capacity, /*UNDNAME_NAME_ONLY*/0x1000) != 0)
			{
				return sb.ToString();
			}
			return name;
		}

		public void SetProcessDpiAwareness()
		{
			if (WinUtil.IsAtLeastWindows10)
			{
				SetProcessDpiAwareness(ProcessDpiAwareness.SystemAware);
			}
			else if (WinUtil.IsAtLeastWindowsVista)
			{
				SetProcessDPIAware();
			}
		}
	}
}
