using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	abstract class BaseFunctionPtrNode : BaseNode
	{
		private IntPtr address = IntPtr.Zero;
		private readonly List<string> assembledCode = new List<string>();

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => IntPtr.Size;

		public override string GetToolTipText(HotSpot spot, Memory memory)
		{
			Contract.Requires(spot != null);
			Contract.Requires(memory != null);

			var ptr = memory.ReadObject<IntPtr>(Offset);

			DisassembleRemoteCode(memory, ptr);

			return string.Join("\n", assembledCode);
		}

		public int Draw(ViewInfo view, int x, int y, string type, string name)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(name != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TXOFFSET;

			x = AddIcon(view, x, y, Icons.Function, -1, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			var tx = x;

			x = AddText(view, x, y, Program.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NameId, name) + view.Font.Width;

			x = AddOpenClose(view, x, y) + view.Font.Width;

			x = AddComment(view, x, y);

			if (levelsOpen[view.Level])
			{
				var ptr = view.Memory.ReadObject<IntPtr>(Offset);

				DisassembleRemoteCode(view.Memory, ptr);

				foreach (var line in assembledCode)
				{
					y += view.Font.Height;

					AddText(view, tx, y, Program.Settings.NameColor, -1, line);
				}
			}

			return y + view.Font.Height;
		}

		private void DisassembleRemoteCode(Memory memory, IntPtr address)
		{
			Contract.Requires(memory != null);

			if (this.address != address)
			{
				assembledCode.Clear();

				this.address = address;

				if (!address.IsNull() && memory.Process.IsValid)
				{
					memory.Process.NativeHelper.DisassembleRemoteCode(
						memory.Process.Process.Handle,
						address,
						60,
#if WIN64
						(a, l, i) => assembledCode.Add($"{a.ToString("X08")} {i}")
#else
						(a, l, i) => assembledCode.Add($"{a.ToString("X04")} {i}")
#endif
					);
				}
			}
		}
	}
}
