using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;
using ReClassNET.Memory;
using ReClassNET.Util;
using ReClassNET.Logger;

namespace ReClassNET.Plugins
{
	internal sealed class PluginManager : IEnumerable<PluginInfo>
	{
		private readonly List<PluginInfo> plugins = new List<PluginInfo>();

		private readonly IPluginHost host = null;
		private readonly NativeHelper nativeHelper;

		public PluginManager(IPluginHost host, NativeHelper nativeHelper)
		{
			Contract.Requires(host != null);

			this.host = host;
			this.nativeHelper = nativeHelper;
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
			Contract.Requires(files != null);
			Contract.Requires(logger != null);

			foreach (var fi in files)
			{
				FileVersionInfo fvi = null;
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
						nativeHelper.InintializeNativeModule(pi.NativeHandle);
					}

					if (!pi.NativeHandle.IsNull())
					{
						nativeHelper.RegisterProvidedNativeMethods(pi.NativeHandle, pi.Name);
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

			var handle = NativeMethods.LoadLibrary(filePath);
			if (handle.IsNull())
			{
				throw new FileLoadException();
			}
			return handle;
		}
	}
}
