using System;
using System.Diagnostics.Contracts;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public class ProcessInfo : IDisposable
	{
		private readonly object sync = new object();

		private readonly NativeHelper nativeHelper;

		private IntPtr handle;

		public int Id { get; }
		public IntPtr Handle => Open();
		public string Name { get; }
		public string Path { get; }

		public ProcessInfo(NativeHelper nativeHelper, int id, string name, string path)
		{
			Contract.Requires(nativeHelper != null);
			Contract.Requires(name != null);
			Contract.Requires(path != null);

			this.nativeHelper = nativeHelper;

			Id = id;
			Name = name;
			Path = path;
		}

		public void Dispose()
		{
			Close();
		}

		public IntPtr Open()
		{
			if (handle.IsNull())
			{
				lock (sync)
				{
					if (handle.IsNull())
					{
						handle = nativeHelper.OpenRemoteProcess(Id, NativeMethods.PROCESS_ALL_ACCESS);
					}
				}
			}
			return handle;
		}

		public void Close()
		{
			if (!handle.IsNull())
			{
				lock (sync)
				{
					if (!handle.IsNull())
					{
						nativeHelper.CloseRemoteProcess(handle);

						handle = IntPtr.Zero;
					}
				}
			}
		}
	}
}
