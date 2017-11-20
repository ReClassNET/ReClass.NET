using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Native;

namespace ReClassNET.Memory
{
	public class ProcessInfo
	{
		public IntPtr Id { get; }
		public string Name { get; }
		public string Path { get; }
		public Image Icon => icon.Value;

		private readonly Lazy<Image> icon;

		public ProcessInfo(IntPtr id, string name, string path)
		{
			Contract.Requires(name != null);
			Contract.Requires(path != null);

			Id = id;
			Name = name;
			Path = path;
			icon = new Lazy<Image>(() =>
			{
				using (var i = NativeMethods.GetIconForFile(Path))
				{
					return i?.ToBitmap();
				}
			});
		}
	}
}
