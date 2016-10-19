using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;

namespace ReClassNET.Plugins
{
	internal sealed class PluginManager : IEnumerable<PluginInfo>
	{
		private readonly List<PluginInfo> plugins = new List<PluginInfo>();

		private readonly IPluginHost host = null;

		public PluginManager(IPluginHost host)
		{
			Contract.Requires(host != null);

			this.host = host;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return plugins.GetEnumerator();
		}
		
		public IEnumerator<PluginInfo> GetEnumerator()
		{
			return plugins.GetEnumerator();
		}

		public void LoadAllPlugins(string path)
		{
			Contract.Requires(host != null);

			try
			{
				if (!Directory.Exists(path))
				{
					return;
				}

				var directory = new DirectoryInfo(path);

				LoadPlugins(directory.GetFiles("*.dll"));

				LoadPlugins(directory.GetFiles("*.exe"));
			}
			catch
			{

			}
		}

		private void LoadPlugins(IEnumerable<FileInfo> files)
		{
			Contract.Requires(files != null);

			foreach (var fi in files)
			{
				FileVersionInfo fvi = null;
				try
				{
					fvi = FileVersionInfo.GetVersionInfo(fi.FullName);

					if (fvi == null || (fvi.ProductName != PluginInfo.PluginName && fvi.ProductName != PluginInfo.PluginNativeName))
					{
						continue;
					}
				}
				catch
				{
					continue;
				}

				try
				{
					var pi = new PluginInfo(fi.FullName, fvi);
					if (!pi.IsNative)
					{
						pi.Interface = CreatePluginInstance(pi.FilePath);

						pi.NativeHandle = Marshal.GetHINSTANCE(pi.Interface.GetType().Module);
					}
					else
					{
						pi.NativeHandle = CreateNativePluginInstance(pi.FilePath);
					}

					if (!pi.IsNative)
					{
						if (!pi.Interface.Initialize(host))
						{
							continue;
						}
					}
					else
					{
						//call native initialize
					}

					plugins.Add(pi);
				}
				catch (Exception ex)
				{
					ex.ShowDialog();
				}
			}
		}

		public void UnloadAllPlugins()
		{
			foreach (var plugin in plugins)
			{
				if (plugin.Interface != null)
				{
					try
					{
						plugin.Interface.Terminate();
					}
					catch
					{

					}
				}
				else if (!plugin.NativeHandle.IsNull())
				{
					Natives.FreeLibrary(plugin.NativeHandle);
				}
			}

			plugins.Clear();
		}

		private static Plugin CreatePluginInstance(string filePath)
		{
			Contract.Requires(filePath != null);

			var type = Path.GetFileNameWithoutExtension(filePath);
			type = type + "." + type + "Ext";

			var handle = Activator.CreateInstanceFrom(filePath, type);

			var plugin = (handle.Unwrap() as Plugin);
			if (plugin == null)
			{
				throw new FileLoadException();
			}
			return plugin;
		}

		private static IntPtr CreateNativePluginInstance(string filePath)
		{
			Contract.Requires(filePath != null);

			var handle = Natives.LoadLibrary(filePath);
			if (handle.IsNull())
			{
				throw new FileLoadException();
			}
			return handle;
		}
	}
}
