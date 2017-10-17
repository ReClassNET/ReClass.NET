using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ReClassNET.Native
{
	internal class NativeMethodsUnix : INativeMethods
	{
		#region Imports

		private const int RTLD_NOW = 2;

		[DllImport("libdl.so")]
		private static extern IntPtr dlopen(string fileName, int flags);

		[DllImport("libdl.so")]
		private static extern IntPtr dlsym(IntPtr handle, string symbol);

		[DllImport("libdl.so")]
		private static extern int dlclose(IntPtr handle);

		#endregion

		public IntPtr LoadLibrary(string fileName)
		{
			return dlopen(fileName, RTLD_NOW);
		}

		public IntPtr GetProcAddress(IntPtr handle, string name)
		{
			// Warning: dlsym could return IntPtr.Zero to a valid function.
			// Error checking with dlerror is needed but we treat IntPtr.Zero as error value...

			return dlsym(handle, name);
		}

		public void FreeLibrary(IntPtr handle)
		{
			dlclose(handle);
		}

		public Icon GetIconForFile(string path)
		{
			return null;
		}

		public void EnableDebugPrivileges()
		{

		}

		public string UndecorateSymbolName(string name)
		{
			return name;
		}

		public void SetProcessDpiAwareness()
		{

		}

		public bool RegisterExtension(string fileExtension, string extensionId, string applicationPath, string applicationName)
		{
			return false;
		}

		public void UnregisterExtension(string fileExtension, string extensionId)
		{

		}
	}
}
