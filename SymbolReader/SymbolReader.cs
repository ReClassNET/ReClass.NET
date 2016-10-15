using Dia2Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.SymbolReader
{
	class SymbolReader : IDisposable
	{
		private ComDisposableWrapper<DiaSource> diaSource;
		private ComDisposableWrapper<IDiaSession> diaSession;

		private string searchPath;

		private IntPtr moduleBase;

		public SymbolReader(string searchPath)
		{
			diaSource = new ComDisposableWrapper<DiaSource>(new DiaSource());

			this.searchPath = searchPath;
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

		public static SymbolReader FromModule(RemoteProcess.Module module, string searchPath)
		{
			Contract.Requires(module != null);

			var reader = new SymbolReader(searchPath)
			{
				moduleBase = module.Start
			};
			reader.LoadDataForModule(module.Path);
			return reader;
		}

		private void LoadDataForModule(string path)
		{
			diaSource.Interface.loadDataForExe(path, searchPath, null);
			var error = diaSource.Interface.lastError;

			IDiaSession session;
			diaSource.Interface.openSession(out session);

			diaSession = new ComDisposableWrapper<IDiaSession>(session);
		}

		public bool LoadSymbolData(string path)
		{
			if (Path.GetExtension(path).ToLower() == ".pdb")
			{
				diaSource.Interface.loadDataFromPdb(path);
			}
			else
			{
				diaSource.Interface.loadDataForExe(path, null, null);
			}

			return false;
		}

		public string GetSymbolStringWithVA(IntPtr address)
		{
			var rva = address.Sub(moduleBase);

			IDiaSymbol diaSymbol;
			diaSession.Interface.findSymbolByRVA((uint)rva.ToInt32(), SymTagEnum.SymTagNull, out diaSymbol);
			if (diaSymbol != null)
			{
				using (var symbol = new ComDisposableWrapper<IDiaSymbol>(diaSymbol))
				{
					var sb = new StringBuilder();
					ReadSymbol(diaSymbol, sb);
					return sb.ToString();
				}
			}
			return null;
		}

		private void ReadSymbol(IDiaSymbol symbol, StringBuilder sb)
		{
			var result = string.Empty;

			switch ((SymTagEnum)symbol.symTag)
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

			ReadSymbolType(symbol, sb);
			ReadName(symbol, sb);

			/*

	case SymTagLabel:
		ReadLocation(pSymbol, outString);
		//outString += _T(", ");
		//wprintf(L", ");
		ReadName(pSymbol, outString);
		break;

	case SymTagEnum:
	case SymTagTypedef:
	case SymTagUDT:
	case SymTagBaseClass:
		ReadUDT(pSymbol, outString);
		break;

	case SymTagFuncDebugStart:
	case SymTagFuncDebugEnd:
		//ReadLocation(pSymbol, outString);
		break;

	case SymTagFunctionArgType:
	case SymTagFunctionType:
	case SymTagPointerType:
	case SymTagArrayType:
	case SymTagBaseType:
		if (pSymbol->get_type(&pType) == S_OK) 
		{
			ReadType(pType, outString);
			pType->Release();
		}
		//putwchar(L'\n');
		break;

	case SymTagThunk:
		//PrintThunk(pSymbol);
		break;

	case SymTagCallSite:
		//PrintCallSiteInfo(pSymbol);
		break;

	case SymTagHeapAllocationSite:
		//PrintHeapAllocSite(pSymbol);
		break;

	case SymTagCoffGroup:
		//PrintCoffGroup(pSymbol);
		break;

	default:
		ReadSymbolType(pSymbol, outString);
		ReadName(pSymbol, outString);

		//if (pSymbol->get_type(&pType) == S_OK)
		//{
		//	outString += _T(" type ");
		//	//wprintf(L" has type ");
		//	ReadType(pType, outString);
		//	pType->Release();
		//}
	}

	if ((dwSymTag == SymTagUDT) || (dwSymTag == SymTagAnnotation))
	{
		IDiaEnumSymbols *pEnumChildren;

		//putwchar(L'\n');

		if (SUCCEEDED(pSymbol->findChildren(SymTagNull, NULL, nsNone, &pEnumChildren))) 
		{
			IDiaSymbol *pChild;
			ULONG celt = 0;

			while (SUCCEEDED(pEnumChildren->Next(1, &pChild, &celt)) && (celt == 1)) 
			{
				ReadSymbol(pChild, outString);
				pChild->Release();
			}

			pEnumChildren->Release();
		}
	}
	//putwchar(L'\n');
}*/
		}

		private void ReadSymbolType(IDiaSymbol symbol, StringBuilder sb)
		{
			if (symbol.type != null)
			{
				using (var type = new ComDisposableWrapper<IDiaSymbol>(symbol.type))
				{
					ReadType(type.Interface, sb);
				}
			}
		}

		private void ReadType(IDiaSymbol symbole, StringBuilder sb)
		{
			return;
		}

		private void ReadName(IDiaSymbol symbol, StringBuilder sb)
		{
			if (string.IsNullOrEmpty(symbol.name))
			{
				return;
			}

			if (!string.IsNullOrEmpty(symbol.undecoratedName))
			{
				if (symbol.name != symbol.undecoratedName)
				{
					sb.AppendFormat("{0} ({1})", symbol.undecoratedName, symbol.name);
				}
			}

			sb.Append(symbol.name);
		}

		private void ReadData(IDiaSymbol symbol, StringBuilder sb)
		{
			throw new NotImplementedException();
		}
	}
}
