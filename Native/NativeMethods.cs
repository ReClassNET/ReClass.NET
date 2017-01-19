using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace ReClassNET.Native
{
	public static class NativeMethods
	{
		private static readonly INativeMethods nativeMethods;

		static NativeMethods()
		{
			if (IsUnix())
			{
				nativeMethods = new NativeMethodsUnix();
			}
			else
			{
				nativeMethods = new NativeMethodsWindows();
			}
		}

		private static bool? isUnix;
		public static bool IsUnix()
		{
			if (isUnix.HasValue)
			{
				return isUnix.Value;
			}

			var p = GetPlatformID();

			isUnix = (p == PlatformID.Unix) || (p == PlatformID.MacOSX) || ((int)p == 128);

			return isUnix.Value;
		}

		private static PlatformID? plattformId;
		public static PlatformID GetPlatformID()
		{
			if (plattformId.HasValue)
			{
				return plattformId.Value;
			}

			plattformId = Environment.OSVersion.Platform;

			// TODO: Mono returns PlatformID.Unix on Mac OS X

			return plattformId.Value;
		}

		public static IntPtr LoadLibrary(string name)
		{
			Contract.Requires(name != null);

			return nativeMethods.LoadLibrary(name);
		}

		public static IntPtr GetProcAddress(IntPtr handle, string name)
		{
			Contract.Requires(name != null);

			return nativeMethods.GetProcAddress(handle, name);
		}

		public static void FreeLibrary(IntPtr handle)
		{
			nativeMethods.FreeLibrary(handle);
		}

		public static Icon GetIconForFile(string path)
		{
			Contract.Requires(path != null);

			return nativeMethods.GetIconForFile(path);
		}

		public static void EnableDebugPrivileges()
		{
			nativeMethods.EnableDebugPrivileges();
		}

		public static string UndecorateSymbolName(string name)
		{
			return nativeMethods.UndecorateSymbolName(name);
		}

		public static void SetProcessDpiAwareness()
		{
			nativeMethods.SetProcessDpiAwareness();
		}
	}
}
