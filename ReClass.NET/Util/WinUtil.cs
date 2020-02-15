using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security.Principal;
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

		//from https://stackoverflow.com/a/11660205
		public static bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

		static WinUtil()
		{
			var os = Environment.OSVersion;
			var v = os.Version;

			IsWindows9x = os.Platform == PlatformID.Win32Windows;
			IsWindows2000 = v.Major == 5 && v.Minor == 0;
			IsWindowsXP = v.Major == 5 && v.Minor == 1;

			IsAtLeastWindows2000 = v.Major >= 5;
			IsAtLeastWindowsVista = v.Major >= 6;
			IsAtLeastWindows7 = v.Major >= 7 || v.Major == 6 && v.Minor >= 1;
			IsAtLeastWindows8 = v.Major >= 7 || v.Major == 6 && v.Minor >= 2;

			try
			{
				using (var rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false))
				{
					if (rk != null)
					{
						var str = rk.GetValue("CurrentMajorVersionNumber", string.Empty)?.ToString();
						if (uint.TryParse(str, out var u))
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

		/// <summary>Executes the a process with elevated permissions.</summary>
		/// <param name="applicationPath"> The executable path.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		public static bool RunElevated(string applicationPath, string arguments)
		{
			Contract.Requires(applicationPath != null);

			try
			{
				var processStartInfo = new ProcessStartInfo
				{
					FileName = applicationPath,
					UseShellExecute = true,
					WindowStyle = ProcessWindowStyle.Normal
				};
				if (arguments != null)
				{
					processStartInfo.Arguments = arguments;
				}

				if (IsAtLeastWindowsVista)
				{
					processStartInfo.Verb = "runas";
				}

				Process.Start(processStartInfo);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}
	}
}
