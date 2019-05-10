using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using ReClassNET.Extensions;
using ReClassNET.Util;

namespace ReClassNET.Native
{
	internal class NativeMethodsWindows : INativeMethods
	{
		#region Imports

		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern bool FreeLibrary(IntPtr hModule);

		private const uint SHGFI_ICON = 0x100;
		private const uint SHGFI_LARGEICON = 0x0;
		private const uint SHGFI_SMALLICON = 0x1;

		[StructLayout(LayoutKind.Sequential)]
		private struct SHFILEINFO
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
		private struct LUID
		{
			public uint LowPart;
			public int HighPart;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct TOKEN_PRIVILEGES
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

		[DllImport("shell32.dll")]
		private static extern void SHChangeNotify(int wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

		private const int SHCNE_ASSOCCHANGED = 0x08000000;
		private const uint SHCNF_IDLIST = 0x0000;

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int nMsg, IntPtr wParam, IntPtr lParam);

		private const int BCM_SETSHIELD = 0x160C;

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

		public bool RegisterExtension(string fileExtension, string extensionId, string applicationPath, string applicationName)
		{
			try
			{
				var classesRoot = Registry.ClassesRoot;

				using (var fileExtensionKey = classesRoot.CreateSubKey(fileExtension))
				{
					fileExtensionKey?.SetValue(string.Empty, extensionId, RegistryValueKind.String);
				}

				using (var extensionInfoKey = classesRoot.CreateSubKey(extensionId))
				{
					extensionInfoKey?.SetValue(string.Empty, applicationName, RegistryValueKind.String);

					using (var icon = extensionInfoKey?.CreateSubKey("DefaultIcon"))
					{
						if (applicationPath.IndexOfAny(new[] { ' ', '\t' }) < 0)
						{
							applicationPath = "\"" + applicationPath + "\"";
						}

						icon?.SetValue(string.Empty, "\"" + applicationPath + "\",0", RegistryValueKind.String);
					}

					using (var shellKey = extensionInfoKey?.CreateSubKey("shell"))
					{
						using (var openKey = shellKey?.CreateSubKey("open"))
						{
							openKey?.SetValue(string.Empty, $"&Open with {applicationName}", RegistryValueKind.String);

							using (var commandKey = openKey?.CreateSubKey("command"))
							{
								commandKey?.SetValue(string.Empty, $"\"{applicationPath}\" \"%1\"", RegistryValueKind.String);
							}
						}
					}
				}

				ShChangeNotify();

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void UnregisterExtension(string fileExtension, string extensionId)
		{
			try
			{
				var classesRoot = Registry.ClassesRoot;

				classesRoot.DeleteSubKeyTree(fileExtension);
				classesRoot.DeleteSubKeyTree(extensionId);

				ShChangeNotify();
			}
			catch (Exception)
			{
				
			}
		}

		private static void ShChangeNotify()
		{
			try
			{
				SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
			}
			catch (Exception)
			{
				
			}
		}

		public static void SetButtonShield(Button button, bool setShield)
		{
			Contract.Requires(button != null);

			try
			{
				if (button.FlatStyle != FlatStyle.System)
				{
					button.FlatStyle = FlatStyle.System;
				}

				var h = button.Handle;
				if (h == IntPtr.Zero)
				{
					return;
				}

				SendMessage(h, BCM_SETSHIELD, IntPtr.Zero, (IntPtr)(setShield ? 1 : 0));
			}
			catch (Exception)
			{
				
			}
		}
	}
}
