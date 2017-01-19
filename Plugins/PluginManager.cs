using System;
using System.Collections;
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
	internal sealed class PluginManager : IEnumerable<PluginInfo>
	{
		private readonly List<PluginInfo> plugins = new List<PluginInfo>();

		private readonly IPluginHost host;
		private readonly CoreFunctionsManager coreFunctions;

		public PluginManager(IPluginHost host, CoreFunctionsManager coreFunctions)
		{
			Contract.Requires(host != null);

			this.host = host;
			this.coreFunctions = coreFunctions;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return plugins.GetEnumerator();
		}
		
		public IEnumerator<PluginInfo> GetEnumerator()
		{
			return plugins.GetEnumerator();
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

				LoadPlugins(directory.GetFiles("*.dll"), logger);

				LoadPlugins(directory.GetFiles("*.exe"), logger);
			}
			catch (Exception ex)
			{
				logger.Log(ex);
			}
		}

		private void LoadPlugins(IEnumerable<FileInfo> files, ILogger logger)
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

					if (fvi.ProductName != PluginInfo.PluginName && fvi.ProductName != PluginInfo.PluginNativeName)
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
						coreFunctions.RegisterFunctions(
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
					NativeMethods.FreeLibrary(plugin.NativeHandle);
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

			var plugin = handle.Unwrap() as Plugin;
			if (plugin == null)
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
