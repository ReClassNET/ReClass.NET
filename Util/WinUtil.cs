using System;
using Microsoft.Win32;

namespace ReClassNET.Util
{
	public static class WinUtil
	{
		public static bool IsWindows9x { get; }

		public static bool IsWindows2000 { get; }

		public static bool IsWindowsXP { get; }

		public static bool IsAtLeastWindows2000 { get; }

		public static bool IsAtLeastWindowsVista { get; }

		public static bool IsAtLeastWindows7 { get; }

		public static bool IsAtLeastWindows8 { get; }

		public static bool IsAtLeastWindows10 { get; }

		static WinUtil()
		{
			OperatingSystem os = Environment.OSVersion;
			Version v = os.Version;

			IsWindows9x = os.Platform == PlatformID.Win32Windows;
			IsWindows2000 = v.Major == 5 && v.Minor == 0;
			IsWindowsXP = v.Major == 5 && v.Minor == 1;

			IsAtLeastWindows2000 = v.Major >= 5;
			IsAtLeastWindowsVista = v.Major >= 6;
			IsAtLeastWindows7 = v.Major >= 7 || (v.Major == 6 && v.Minor >= 1);
			IsAtLeastWindows8 = v.Major >= 7 || (v.Major == 6 && v.Minor >= 2);

			try
			{
				using (var rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false))
				{
					if (rk != null)
					{
						var str = rk.GetValue("CurrentMajorVersionNumber", string.Empty)?.ToString();
						uint u;
						if (uint.TryParse(str, out u))
						{
							IsAtLeastWindows10 = u >= 10;
						}
					}
				}
			}
			catch
			{
				
			}
		}
	}
}
