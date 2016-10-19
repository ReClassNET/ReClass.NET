using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;

namespace ReClassNET.Plugins
{
	class PluginInfo
	{
		public const string PluginName = "ReClass.NET Plugin";
		public const string PluginNativeName = "ReClass.NET Native Plugin";

		public string FilePath { get; }

		public string FileVersion { get; }

		public string Name { get; }

		public string Description { get; }

		public string Author { get; }

		public bool IsNative { get; }

		public Plugin Interface { get; set; }

		public IntPtr NativeHandle { get; set; }

		public PluginInfo(string filePath, FileVersionInfo versionInfo)
		{
			Contract.Requires(!string.IsNullOrEmpty(filePath));
			Contract.Requires(versionInfo != null);

			FilePath = filePath;
			IsNative = versionInfo.ProductName == PluginNativeName;

			FileVersion = (versionInfo.FileVersion ?? string.Empty).Trim();

			Description = (versionInfo.Comments ?? string.Empty).Trim();
			Author = (versionInfo.CompanyName ?? string.Empty).Trim();

			Name = (versionInfo.FileDescription ?? string.Empty).Trim();
			if (string.IsNullOrEmpty(Name))
			{
				Name = Path.GetFileNameWithoutExtension(FilePath);
			}
		}
	}
}
