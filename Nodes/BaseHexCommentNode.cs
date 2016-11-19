using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public abstract class BaseHexCommentNode : BaseHexNode
	{
		protected int AddComment(ViewInfo view, int x, int y, float fvalue, IntPtr ivalue, UIntPtr uvalue)
		{
			Contract.Requires(view != null);

			if (Program.Settings.ShowCommentFloat)
			{
				x = AddText(view, x, y, Program.Settings.ValueColor, HotSpot.NoneId, $"({(fvalue > -99999.0f && fvalue < 99999.0f ? fvalue : 0.0f):0.000})");
			}
			if (Program.Settings.ShowCommentInteger)
			{
				if (ivalue == IntPtr.Zero)
				{
					x = AddText(view, x, y, Program.Settings.ValueColor, HotSpot.NoneId, "(0)");
				}
				else
				{
					x = AddText(view, x, y, Program.Settings.ValueColor, HotSpot.NoneId, $"({ivalue.ToInt64()}|0x{uvalue.ToUInt64():X})");
				}
			}

			var namedAddress = view.Memory.Process.GetNamedAddress(ivalue);
			if (!string.IsNullOrEmpty(namedAddress))
			{
				x += view.Font.Width;

				if (Program.Settings.ShowCommentPointer)
				{
					x = AddText(view, x, y, Program.Settings.OffsetColor, HotSpot.NoneId, $"-> {namedAddress}") + view.Font.Width;
				}

				if (Program.Settings.ShowCommentRtti)
				{
					var rtti = view.Memory.Process.ReadRemoteRuntimeTypeInformation(ivalue);
					if (!string.IsNullOrEmpty(rtti))
					{
						x = AddText(view, x, y, Program.Settings.OffsetColor, HotSpot.ReadOnlyId, rtti) + view.Font.Width;
					}
				}

				if (Program.Settings.ShowCommentSymbol)
				{
					var module = view.Memory.Process.GetModuleToPointer(ivalue);
					if (module != null)
					{
						var symbols = view.Memory.Process.Symbols.GetSymbolsForModule(module);
						if (symbols != null)
						{
							var symbol = symbols.GetSymbolString(ivalue, module);
							if (!string.IsNullOrEmpty(symbol))
							{
								x = AddText(view, x, y, Program.Settings.OffsetColor, HotSpot.ReadOnlyId, symbol) + view.Font.Width;
							}
						}
					}
				}

				if (Program.Settings.ShowCommentString)
				{
					var data = view.Memory.Process.ReadRemoteMemory(ivalue, 64);

					// First check if it could be an UTF8 string and if not try UTF16.
					if (data.Take(IntPtr.Size).InterpretAsUTF8().IsPrintableData())
					{
						var text = new string(Encoding.UTF8.GetChars(data).TakeWhile(c => c != 0).ToArray());
						x = AddText(view, x, y, Program.Settings.TextColor, HotSpot.ReadOnlyId, $"'{text}'") + view.Font.Width;
					}
					else if(data.Take(IntPtr.Size * 2).InterpretAsUTF16().IsPrintableData())
					{
						var text = new string(Encoding.Unicode.GetChars(data).TakeWhile(c => c != 0).ToArray());
						x = AddText(view, x, y, Program.Settings.TextColor, HotSpot.ReadOnlyId, $"L'{text}'") + view.Font.Width;
					}
				}

				if (Program.Settings.ShowCommentPluginInfo)
				{
					foreach (var reader in NodeInfoReader)
					{
						var info = reader.ReadNodeInfo(this, ivalue, view.Memory);
						if (info != null)
						{
							x = AddText(view, x, y, Program.Settings.PluginColor, HotSpot.ReadOnlyId, info) + view.Font.Width;
						}
					}
				}
			}

			return x;
		}
	}
}
