using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Dia2Lib;
using Microsoft.Win32;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace ReClassNET.Symbols
{
	class DiaUtil : IDisposable
	{
		public readonly IDiaDataSource _IDiaDataSource;
		public readonly IDiaSession _IDiaSession;

		public DiaUtil(string pdbName)
		{
			Contract.Requires(pdbName != null);

			_IDiaDataSource = new DiaSource();
			_IDiaDataSource.loadDataFromPdb(pdbName);
			_IDiaDataSource.openSession(out _IDiaSession);
		}

		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				Marshal.ReleaseComObject(_IDiaSession);
				Marshal.ReleaseComObject(_IDiaDataSource);

				disposedValue = true;
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
							if (debuggerKey != null)
							{
								var symbolCacheDir = debuggerKey.GetValue("SymbolCacheDir") as string;
								if (symbolCacheDir != null)
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
		}

		public void TryResolveSymbolsForModule(Module module)
		{
			Contract.Requires(module != null);

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

			var moduleName = Path.GetFileName(path).ToLower();

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

			var name = module.Name.ToLower();

			lock (symbolReaders)
			{
				SymbolReader reader;
				if (!symbolReaders.TryGetValue(name, out reader))
				{
					name = Path.ChangeExtension(name, ".pdb");

					symbolReaders.TryGetValue(name, out reader);
				}
				return reader;
			}
		}
	}
}
