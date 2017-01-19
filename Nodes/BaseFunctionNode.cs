using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseFunctionNode : BaseNode
	{
		protected class FunctionNodeInstruction
		{
			public string Address;
			public string Data;
			public string Instruction;
		}

		protected IntPtr address = IntPtr.Zero;
		protected readonly List<FunctionNodeInstruction> instructions = new List<FunctionNodeInstruction>();

		protected int DrawInstructions(ViewInfo view, int tx, int y)
		{
			Contract.Requires(view != null);

			var minWidth = 26 * view.Font.Width;

			foreach (var instruction in instructions)
			{
				y += view.Font.Height;

				using (var brush = new SolidBrush(view.Settings.HiddenColor))
				{
					var x = AddText(view, tx, y, view.Settings.AddressColor, HotSpot.ReadOnlyId, instruction.Address) + 6;

					view.Context.FillRectangle(brush, x, y, 1, view.Font.Height);
					x += 6;

					x = Math.Max(AddText(view, x, y, view.Settings.HexColor, HotSpot.ReadOnlyId, instruction.Data) + 6, x + minWidth);

					view.Context.FillRectangle(brush, x, y, 1, view.Font.Height);
					x += 6;

					AddText(view, x, y, view.Settings.ValueColor, HotSpot.ReadOnlyId, instruction.Instruction);
				}
			}

			return y;
		}

		protected void DisassembleRemoteCode(MemoryBuffer memory, IntPtr address, out int memorySize)
		{
			Contract.Requires(memory != null);

			memorySize = 0;

			var disassembler = new Disassembler(memory.Process.CoreFunctions);
			foreach (var instruction in disassembler.RemoteDisassembleFunction(memory.Process, address, 8192))
			{
				memorySize += instruction.Length;

				instructions.Add(new FunctionNodeInstruction
				{
					Address = instruction.Address.ToString(Constants.StringHexFormat),
					Data = string.Join(" ", instruction.Data.Take(instruction.Length).Select(b => $"{b:X2}")),
					Instruction = instruction.Instruction
				});
			}
		}
	}
}
