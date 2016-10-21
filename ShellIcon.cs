using System.Diagnostics.Contracts;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ReClassNET
{
	public static class ShellIcon
	{
		public static Icon GetSmallIcon(string filePath)
		{
			Contract.Requires(filePath != null);

			return GetIcon(filePath, NativeMethods.SHGFI_SMALLICON);
		}

		public static Icon GetLargeIcon(string filePath)
		{
			Contract.Requires(filePath != null);

			return GetIcon(filePath, NativeMethods.SHGFI_LARGEICON);
		}

		private static Icon GetIcon(string filePath, uint flags)
		{
			Contract.Requires(filePath != null);

			var shinfo = new NativeMethods.SHFILEINFO();
			if (NativeMethods.SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), NativeMethods.SHGFI_ICON | flags).ToInt32() > 0)
			{
				var icon = Icon.FromHandle(shinfo.hIcon).Clone() as Icon;
				NativeMethods.DestroyIcon(shinfo.hIcon);
				return icon;
			}

			return null;
		}
	}
}
