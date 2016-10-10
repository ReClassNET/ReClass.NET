using System.Drawing;
using System.Runtime.InteropServices;

namespace ReClassNET
{
	public static class ShellIcon
	{
		public static Icon GetSmallIcon(string fileName)
		{
			return GetIcon(fileName, Natives.SHGFI_SMALLICON);
		}

		public static Icon GetLargeIcon(string fileName)
		{
			return GetIcon(fileName, Natives.SHGFI_LARGEICON);
		}

		private static Icon GetIcon(string fileName, uint flags)
		{
			var shinfo = new Natives.SHFILEINFO();
			if (Natives.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Natives.SHGFI_ICON | flags).ToInt32() > 0)
			{
				var icon = Icon.FromHandle(shinfo.hIcon).Clone() as Icon;
				Natives.DestroyIcon(shinfo.hIcon);
				return icon;
			}

			return null;
		}
	}
}
