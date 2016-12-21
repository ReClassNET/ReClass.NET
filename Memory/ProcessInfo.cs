using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Memory
{
	public class ProcessInfo
	{
		public IntPtr Id { get; }
		public string Name { get; }
		public string Path { get; }

		public ProcessInfo(IntPtr id, string path)
		{
			Contract.Requires(path != null);

			Id = id;
			Name = System.IO.Path.GetFileName(path);
			Path = path;
		}
	}
}
