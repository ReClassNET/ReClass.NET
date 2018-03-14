using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Dia2Lib;
using Microsoft.Win32;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET.Symbols
{
	class DiaUtil : IDisposable
	{
		public readonly IDiaDataSource diaDataSource;
		public readonly IDiaSession diaSession;

		public DiaUtil(string pdbName)
		{
			Contract.Requires(pdbName != null);

			diaDataSource = new DiaSource();
			diaDataSource.loadDataFromPdb(pdbName);
			diaDataSource.openSession(out diaSession);
		}

		private bool isDisposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				Marshal.ReleaseComObject(diaSession);
				Marshal.ReleaseComObject(diaDataSource);

				isDisposed = true;
			}
		}

		~DiaUtil()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	public class SymbolStore
	{
		private const string BlackListFile = "blacklist.txt";

		public string SymbolCachePath { get; private set; } = "./SymbolsCache";

		public string SymbolDownloadPath { get; set; } = "http://msdl.microsoft.com/download/symbols";

		public string SymbolSearchPath => $"srv*{SymbolCachePath}*{SymbolDownloadPath}";

		private readonly Dictionary<string, SymbolReader> symbolReaders = new Dictionary<string, SymbolReader>();

		private readonly HashSet<string> moduleBlacklist = new HashSet<string>();

		public SymbolStore()
		{
			if (NativeMethods.IsUnix())
			{
				// TODO: Are there symbol files like on windows?

				return;
			}

			ResolveSearchPath();

			var blacklistPath = Path.Combine(SymbolCachePath, BlackListFile);

			if (File.Exists(blacklistPath))
			{
				File.ReadAllLines(Path.Combine(SymbolCachePath, BlackListFile))
					.Select(l => l.Trim().ToLower())
					.ForEach(l => moduleBlacklist.Add(l));
			}
		}

		private void ResolveSearchPath()
		{
			using (var vsKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio"))
			{
				if (vsKey != null)
				{
					foreach (var subKeyName in vsKey.GetSubKeyNames())
					{
						using (var debuggerKey = vsKey.OpenSubKey($@"{subKeyName}\Debugger"))
						{
							if (debuggerKey?.GetValue("SymbolCacheDir") is string symbolCacheDir)
							{
								if (Directory.Exists(symbolCacheDir))
								{
									SymbolCachePath = symbolCacheDir;
								}

								return;
							}
						}
					}
				}
			}
		}

		public void TryResolveSymbolsForModule(Module module)
		{
			Contract.Requires(module != null);
			Contract.Requires(module.Name != null);

			if (NativeMethods.IsUnix())
			{
				return;
			}

			var name = module.Name.ToLower();

			bool isBlacklisted;
			lock (symbolReaders)
			{
				isBlacklisted = moduleBlacklist.Contains(name);
			}

			if (!isBlacklisted)
			{
				try
				{
					SymbolReader.TryResolveSymbolsForModule(module, SymbolSearchPath);
				}
				catch
				{
					lock (symbolReaders)
					{
						moduleBlacklist.Add(name);

						File.WriteAllLines(
							Path.Combine(SymbolCachePath, BlackListFile),
							moduleBlacklist.ToArray()
						);
					}
				}
			}
		}

		public void LoadSymbolsForModule(Module module)
		{
			Contract.Requires(module != null);
			Contract.Requires(module.Name != null);

			if (NativeMethods.IsUnix())
			{
				return;
			}

			var moduleName = module.Name.ToLower();

			bool createNew;
			lock (symbolReaders)
			{
				createNew = !symbolReaders.ContainsKey(moduleName);
			}

			if (createNew)
			{
				var reader = SymbolReader.FromModule(module, SymbolSearchPath);
				
				lock(symbolReaders)
				{
					symbolReaders[moduleName] = reader;
				}
			}
		}

		public void LoadSymbolsFromPDB(string path)
		{
			Contract.Requires(path != null);

			if (NativeMethods.IsUnix())
			{
				return;
			}

			var moduleName = Path.GetFileName(path)?.ToLower();
			if (string.IsNullOrEmpty(moduleName))
			{
				return;
			}

			bool createNew;
			lock (symbolReaders)
			{
				createNew = !symbolReaders.ContainsKey(moduleName);
			}

			if (createNew)
			{
				var reader = SymbolReader.FromDatabase(path);

				lock (symbolReaders)
				{
					symbolReaders[moduleName] = reader;
				}
			}
		}

		public SymbolReader GetSymbolsForModule(Module module)
		{
			Contract.Requires(module != null);
			Contract.Requires(module.Name != null);

			if (NativeMethods.IsUnix())
			{
				return null;
			}

			var name = module.Name.ToLower();

			lock (symbolReaders)
			{
				if (!symbolReaders.TryGetValue(name, out var reader))
				{
					name = Path.ChangeExtension(name, ".pdb");

					symbolReaders.TryGetValue(name, out reader);
				}
				return reader;
			}
		}
	}
}
