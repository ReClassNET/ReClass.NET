using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public class FunctionNode : BaseNode
	{
		private int memorySize = IntPtr.Size;

		private IntPtr address = IntPtr.Zero;
		private readonly List<string> disassembledCode = new List<string>();

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => memorySize;

		public override string GetToolTipText(HotSpot spot, MemoryBuffer memory)
		{
			DisassembleRemoteCode(memory, spot.Address);

			return string.Join("\n", disassembledCode);
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Function, -1, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			var tx = x;

			x = AddText(view, x, y, Program.Settings.TypeColor, HotSpot.NoneId, "Function") + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddOpenClose(view, x, y) + view.Font.Width;

			x = AddComment(view, x, y);

			var ptr = view.Address.Add(Offset);

			DisassembleRemoteCode(view.Memory, ptr);

			if (levelsOpen[view.Level])
			{
				foreach (var line in disassembledCode)
				{
					y += view.Font.Height;

					AddText(view, tx, y, Program.Settings.NameColor, HotSpot.ReadOnlyId, line);
				}
			}

			return y + view.Font.Height;
		}

		public override int CalculateHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var h = view.Font.Height;
			if (levelsOpen[view.Level])
			{
				h += disassembledCode.Count() * view.Font.Height;
			}
			return h;
		}

		private void DisassembleRemoteCode(MemoryBuffer memory, IntPtr address)
		{
			Contract.Requires(memory != null);

			if (this.address != address)
			{
				disassembledCode.Clear();

				this.address = address;

				if (!address.IsNull() && memory.Process.IsValid)
				{
					memorySize = 0;

					memory.Process.NativeHelper.DisassembleRemoteCode(
						memory.Process.Process.Handle,
						address,
						4096,
						(a, l, i) =>
						{
							memorySize += l;
#if WIN64
							disassembledCode.Add($"{a.ToString("X08")} {i}");
#else
							disassembledCode.Add($"{a.ToString("X04")} {i}");
#endif
						}
					);

					ParentNode?.ChildHasChanged(this);
				}
			}
		}
	}
}
