using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace ReClassNET.Native
{
	[ContractClass(typeof(NativeMethodsContract))]
	internal interface INativeMethods
	{
		IntPtr LoadLibrary(string fileName);

		IntPtr GetProcAddress(IntPtr handle, string name);

		void FreeLibrary(IntPtr handle);

		Icon GetIconForFile(string path);

		void EnableDebugPrivileges();

		string UndecorateSymbolName(string name);

		void SetProcessDpiAwareness();

		bool RegisterExtension(string fileExtension, string extensionId, string applicationPath, string applicationName);

		void UnregisterExtension(string fileExtension, string extensionId);
	}

	[ContractClassFor(typeof(INativeMethods))]
	internal abstract class NativeMethodsContract : INativeMethods
	{
		public IntPtr LoadLibrary(string fileName)
		{
			Contract.Requires(fileName != null);

			throw new NotImplementedException();
		}

		public IntPtr GetProcAddress(IntPtr handle, string name)
		{
			Contract.Requires(name != null);

			throw new NotImplementedException();
		}

		public void FreeLibrary(IntPtr handle)
		{
			throw new NotImplementedException();
		}

		public Icon GetIconForFile(string path)
		{
			Contract.Requires(path != null);

			throw new NotImplementedException();
		}

		public void EnableDebugPrivileges()
		{
			throw new NotImplementedException();
		}

		public string UndecorateSymbolName(string name)
		{
			Contract.Requires(name != null);
			Contract.Ensures(Contract.Result<string>() != null);

			throw new NotImplementedException();
		}

		public void SetProcessDpiAwareness()
		{
			throw new NotImplementedException();
		}

		public bool RegisterExtension(string fileExtension, string extensionId, string applicationPath, string applicationName)
		{
			Contract.Requires(!string.IsNullOrEmpty(fileExtension));
			Contract.Requires(!string.IsNullOrEmpty(extensionId));
			Contract.Requires(applicationPath != null);
			Contract.Requires(applicationName != null);

			throw new NotImplementedException();
		}

		public void UnregisterExtension(string fileExtension, string extensionId)
		{
			Contract.Requires(!string.IsNullOrEmpty(fileExtension));
			Contract.Requires(!string.IsNullOrEmpty(extensionId));

			throw new NotImplementedException();
		}
	}
}
