using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Memory
{
	public class ProcessInfo
	{
		public IntPtr Id { get; }
		public string Name { get; }
		public string Path { get; }

		public ProcessInfo(IntPtr id, string name, string path)
		{
			Contract.Requires(name != null);
			Contract.Requires(path != null);

			Id = id;
			Name = name;
			Path = path;
		}
	}
}
