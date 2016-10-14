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
				x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"({(fvalue > -99999.0f && fvalue < 99999.0f ? fvalue : 0.0f):0.000})");
			}
			if (view.Settings.ShowInteger)
			{
				if (ivalue == IntPtr.Zero)
				{
					x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, "(0)");
				}
				else
				{
					x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"({ivalue.ToInt64()}|0x{uvalue.ToUInt64():X})");
				}
			}

			var namedAddress = view.Memory.Process.GetNamedAddress(ivalue);
			if (!string.IsNullOrEmpty(namedAddress))
			{
				x += view.Font.Width;

				if (view.Settings.ShowPointer)
				{
					x = AddText(view, x, y, view.Settings.OffsetColor, HotSpot.NoneId, $"-> {namedAddress}") + view.Font.Width;
				}

				if (view.Settings.ShowRTTI)
				{
					var rtti = view.Memory.Process.ReadRemoteRuntimeTypeInformation(ivalue);
					if (!string.IsNullOrEmpty(rtti))
					{
						x = AddText(view, x, y, view.Settings.OffsetColor, HotSpot.NoneId, rtti);
					}
				}

				if (view.Settings.ShowSymbols)
				{
					var module = view.Memory.Process.Modules.Where(m => ivalue.InRange(m.Start, m.End)).FirstOrDefault();
					if (module != null)
					{
						var symbols = view.Memory.GetSymbolsForModule(null);
						if (symbols != null)
						{
							var symbol = symbols.GetSymbolStringWithVA(ivalue);
							if (!string.IsNullOrEmpty(symbol))
							{
								x = AddText(view, x, y, view.Settings.OffsetColor, HotSpot.NoneId, symbol + " ");
							}
						}
					}
				}

				if (view.Settings.ShowStrings)
				{
					var txt = view.Memory.Process.ReadRemoteRawUTF8String(ivalue, 64);
					if (txt != null)
					{
						if (!txt.Take(4).Where(c => !c.IsPrintable()).Any())
						{
							x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, $"'{txt}'");
						}
					}
				}
			}

			return x;
		}
	}
}
