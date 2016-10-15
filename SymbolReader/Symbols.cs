using Dia2Lib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.SymbolReader
{


	class DiaUtil : IDisposable
	{
		public IDiaDataSource _IDiaDataSource;
		public IDiaSession _IDiaSession;

		public DiaUtil(string pdbName)
		{
			_IDiaDataSource = new DiaSource();
			_IDiaDataSource.loadDataFromPdb(pdbName);
			_IDiaDataSource.openSession(out _IDiaSession);
		}

		private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

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

	class Symbols
	{
		public string SymbolCachePath { get; private set; } = "./SymbolsCache";

		public string SymbolSearchPath => $"srv*{SymbolCachePath}*http://msdl.microsoft.com/download/symbols";

		private Dictionary<string, SymbolReader> symbolReaders = new Dictionary<string, SymbolReader>();

		public Symbols()
		{
			ResolveSearchPath();
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

		public void LoadSymbolsForModule(RemoteProcess.Module module)
		{
			var reader = SymbolReader.FromModule(module, SymbolSearchPath);
			symbolReaders[module.Name] = reader;
		}

		public SymbolReader GetSymbolsForModule(RemoteProcess.Module module)
		{
			SymbolReader reader;
			symbolReaders.TryGetValue(module.Name, out reader);
			return reader;
		}
	}
}
