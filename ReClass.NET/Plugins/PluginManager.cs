using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using ReClassNET.Core;
using ReClassNET.Logger;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET.Plugins
{
	internal sealed class PluginManager
	{
		private readonly List<PluginInfo> plugins = new List<PluginInfo>();

		private readonly IPluginHost host;

		public IEnumerable<PluginInfo> Plugins => plugins;

		public PluginManager(IPluginHost host)
		{
			Contract.Requires(host != null);

			this.host = host;
		}

		public void LoadAllPlugins(string path, ILogger logger)
		{
			Contract.Requires(path != null);
			Contract.Requires(logger != null);

			try
			{
				if (!Directory.Exists(path))
				{
					return;
				}

				var directory = new DirectoryInfo(path);

				LoadPlugins(directory.GetFiles("*.dll"), logger, true);

				LoadPlugins(directory.GetFiles("*.exe"), logger, true);

				LoadPlugins(directory.GetFiles("*.so"), logger, false);
			}
			catch (Exception ex)
			{
				logger.Log(ex);
			}
		}

		private void LoadPlugins(IEnumerable<FileInfo> files, ILogger logger, bool checkProductName)
		{
			// TODO: How to include plugin infos for unix files as they don't have embedded version info.

			Contract.Requires(files != null);
			Contract.Requires(logger != null);

			foreach (var fi in files)
			{
				FileVersionInfo fvi;
				try
				{
					fvi = FileVersionInfo.GetVersionInfo(fi.FullName);

					if (checkProductName && fvi.ProductName != PluginInfo.PluginName && fvi.ProductName != PluginInfo.PluginNativeName)
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

						if (!pi.Interface.Initialize(host))
						{
							continue;
						}
					}
					else
					{
						pi.NativeHandle = CreateNativePluginInstance(pi.FilePath);

						Program.CoreFunctions.RegisterFunctions(
							pi.Name,
							new NativeCoreWrapper(pi.NativeHandle)
						);
					}

					plugins.Add(pi);
				}
				catch (Exception ex)
				{
					logger.Log(ex);
				}
			}
		}

		public void UnloadAllPlugins()
		{
			plugins.ForEach(p => p.Dispose());
			plugins.Clear();
		}

		private static Plugin CreatePluginInstance(string filePath)
		{
			Contract.Requires(filePath != null);

			var type = Path.GetFileNameWithoutExtension(filePath);
			type = type + "." + type + "Ext";

			var handle = Activator.CreateInstanceFrom(filePath, type);

			if (!(handle.Unwrap() is Plugin plugin))
			{
				throw new FileLoadException();
			}
			return plugin;
		}

		private static IntPtr CreateNativePluginInstance(string filePath)
		{
			Contract.Requires(filePath != null);

			var handle = NativeMethods.LoadLibrary(filePath);
			if (handle.IsNull())
			{
				throw new FileLoadException();
			}
			return handle;
		}
	}
}
