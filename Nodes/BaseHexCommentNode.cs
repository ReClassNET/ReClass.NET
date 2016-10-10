using System;
using System.Linq;

namespace ReClassNET.Nodes
{
	abstract class BaseHexCommentNode : BaseHexNode
	{
		protected int AddComment(ViewInfo view, int x, int y, float fvalue, IntPtr ivalue, UIntPtr uvalue)
		{
			if (view.Settings.ShowFloat)
			{
				x = AddText(view, x, y, view.Settings.Value, HotSpot.NoneId, $"({(fvalue > -99999.0f && fvalue < 99999.0f ? fvalue : 0.0f):0.000})");
			}
			if (view.Settings.ShowInteger)
			{
				if (ivalue == IntPtr.Zero)
				{
					x = AddText(view, x, y, view.Settings.Value, HotSpot.NoneId, "(0)");
				}
				else
				{
					x = AddText(view, x, y, view.Settings.Value, HotSpot.NoneId, $"({ivalue.ToInt64()}|0x{uvalue.ToUInt64():X})");
				}
			}

			var namedAddress = view.Memory.GetNamedAddress(ivalue);
			if (!string.IsNullOrEmpty(namedAddress))
			{
				if (view.Settings.ShowPointer)
				{
					x = AddText(view, x, y, view.Settings.Offset, HotSpot.NoneId, $"*->{namedAddress} ");

					if (view.Settings.ShowRTTI)
					{
						x = AddRTTI(view, x, y);
					}

					if (view.Settings.ShowSymbols)
					{
						var moduleName = view.Memory.GetModuleName(ivalue);
						if (!string.IsNullOrEmpty(moduleName))
						{
							var symbols = view.Memory.GetSymbolsForModule(moduleName);
							if (symbols != null)
							{
								var symbol = symbols.GetSymbolStringWithVA(ivalue);
								if (!string.IsNullOrEmpty(symbol))
								{
									x = AddText(view, x, y, view.Settings.Offset, HotSpot.NoneId, symbol + " ");
								}
							}
						}
					}
				}

				if (view.Settings.ShowStrings)
				{
					var txt = view.Memory.Process.ReadUTF8String(ivalue, 64);
					if (txt != null)
					{
						if (txt.Take(8).Where(c => !char.IsControl(c)).Any())
						{
							x = AddText(view, x, y, view.Settings.Text, HotSpot.NoneId, $"'{txt}'");
						}
					}
				}
			}

			return x;
		}
	}
}
