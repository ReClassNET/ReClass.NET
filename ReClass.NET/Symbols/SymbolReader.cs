using System;
using System.Diagnostics.Contracts;
using System.Text;
using Dia2Lib;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET.Symbols
{
	public class SymbolReader : IDisposable
	{
		private ComDisposableWrapper<DiaSource> diaSource;
		private ComDisposableWrapper<IDiaSession> diaSession;

		public SymbolReader()
		{
			diaSource = new ComDisposableWrapper<DiaSource>(new DiaSource());
		}

		protected virtual void Dispose(bool disposing)
		{
			if (diaSource != null)
			{
				diaSource.Dispose();
				diaSource = null;

				if (diaSession != null)
				{
					diaSession.Dispose();
					diaSession = null;
				}
			}
		}

		~SymbolReader()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public static void TryResolveSymbolsForModule(Module module, string searchPath)
		{
			Contract.Requires(module != null);

			using (var diaSource = new ComDisposableWrapper<DiaSource>(new DiaSource()))
			{
				diaSource.Interface.loadDataForExe(module.Path, searchPath, null);
			}
		}

		public static SymbolReader FromModule(Module module, string searchPath)
		{
			Contract.Requires(module != null);

			var reader = new SymbolReader();
			reader.diaSource.Interface.loadDataForExe(module.Path, searchPath, null);
			reader.CreateSession();
			return reader;
		}

		public static SymbolReader FromDatabase(string path)
		{
			Contract.Requires(path != null);

			var reader = new SymbolReader();
			reader.diaSource.Interface.loadDataFromPdb(path);
			reader.CreateSession();
			return reader;
		}

		private void CreateSession()
		{
			Contract.Ensures(diaSession != null);

			diaSource.Interface.openSession(out var session);

			diaSession = new ComDisposableWrapper<IDiaSession>(session);
		}

		public string GetSymbolString(IntPtr address, Module module)
		{
			Contract.Requires(module != null);

			var rva = address.Sub(module.Start);

			diaSession.Interface.findSymbolByRVA((uint)rva.ToInt32(), SymTagEnum.SymTagNull, out var diaSymbol);
			if (diaSymbol != null)
			{
				using (var symbol = new ComDisposableWrapper<IDiaSymbol>(diaSymbol))
				{
					var sb = new StringBuilder();
					ReadSymbol(symbol.Interface, sb);
					return sb.ToString();
				}
			}
			return null;
		}

		private void ReadSymbol(IDiaSymbol symbol, StringBuilder sb)
		{
			Contract.Requires(symbol != null);
			Contract.Requires(sb != null);

			/*switch ((SymTagEnum)symbol.symTag)
			{
				case SymTagEnum.SymTagData:
					ReadData(symbol, sb);
					break;
				case SymTagEnum.SymTagFunction:
					sb.Append(symbol.callingConvention.ToString());
					ReadName(symbol, sb);
					break;
				case SymTagEnum.SymTagBlock:
					sb.AppendFormat("len({0:X08}) ", symbol.length);
					ReadName(symbol, sb);
					break;
			}

			ReadSymbolType(symbol, sb);*/
			ReadName(symbol, sb);
		}

		private void ReadSymbolType(IDiaSymbol symbol, StringBuilder sb)
		{
			Contract.Requires(symbol != null);
			Contract.Requires(sb != null);

			if (symbol.type != null)
			{
				using (var type = new ComDisposableWrapper<IDiaSymbol>(symbol.type))
				{
					ReadType(type.Interface, sb);
				}
			}
		}

		private void ReadType(IDiaSymbol symbol, StringBuilder sb)
		{
			Contract.Requires(symbol != null);
			Contract.Requires(sb != null);

			throw new NotImplementedException();
		}

		private void ReadName(IDiaSymbol symbol, StringBuilder sb)
		{
			Contract.Requires(symbol != null);
			Contract.Requires(sb != null);

			if (string.IsNullOrEmpty(symbol.name))
			{
				return;
			}

			if (!string.IsNullOrEmpty(symbol.undecoratedName))
			{
				// If symbol.name equals symbol.undecoratedName there is some extra stuff which can't get undecorated. Try to fix it.
				if (symbol.name == symbol.undecoratedName)
				{
					var name = symbol.name;
					if (name.StartsWith("@ILT+"))
					{
						var start = name.IndexOf('(');
						if (start != -1)
						{
							name = name.Substring(start + 1, name.Length - 1 - start - 1);
						}
					}
					else if (!name.StartsWith("?"))
					{
						name = '?' + name;
					}

					sb.Append(NativeMethods.UndecorateSymbolName(name).TrimStart('?', ' '));
				}
				else
				{
					sb.Append(symbol.undecoratedName);
				}
			}
			else
			{
				sb.Append(symbol.name);
			}
		}

		private void ReadData(IDiaSymbol symbol, StringBuilder sb)
		{
			Contract.Requires(symbol != null);
			Contract.Requires(sb != null);

			throw new NotImplementedException();
		}
	}
}
