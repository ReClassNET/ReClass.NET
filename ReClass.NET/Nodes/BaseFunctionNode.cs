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
			public string Address { get; set; }
			public string Data { get; set; }
			public string Instruction { get; set; }
		}

		protected IntPtr address = IntPtr.Zero;
		protected readonly List<FunctionNodeInstruction> instructions = new List<FunctionNodeInstruction>();

		protected Size DrawInstructions(ViewInfo view, int tx, int y)
		{
			Contract.Requires(view != null);

			var origY = y;

			var minWidth = 26 * view.Font.Width;
			var maxWidth = 0;

			using (var brush = new SolidBrush(view.Settings.HiddenColor))
			{
				foreach (var instruction in instructions)
				{
					y += view.Font.Height;

					var x = AddText(view, tx, y, view.Settings.AddressColor, HotSpot.ReadOnlyId, instruction.Address) + 6;

					view.Context.FillRectangle(brush, x, y, 1, view.Font.Height);
					x += 6;

					x = Math.Max(AddText(view, x, y, view.Settings.HexColor, HotSpot.ReadOnlyId, instruction.Data) + 6, x + minWidth);

					view.Context.FillRectangle(brush, x, y, 1, view.Font.Height);
					x += 6;

					x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.ReadOnlyId, instruction.Instruction);

					maxWidth = Math.Max(x - tx, maxWidth);
				}
			}

			return new Size(maxWidth, y - origY);
		}

		protected void DisassembleRemoteCode(RemoteProcess process, IntPtr address, out int memorySize)
		{
			Contract.Requires(process != null);

			memorySize = 0;

			var disassembler = new Disassembler(process.CoreFunctions);
			foreach (var instruction in disassembler.RemoteDisassembleFunction(process, address, 8192))
			{
				memorySize += instruction.Length;

				instructions.Add(new FunctionNodeInstruction
				{
					Address = instruction.Address.ToString(Constants.AddressHexFormat),
					Data = string.Join(" ", instruction.Data.Take(instruction.Length).Select(b => $"{b:X2}")),
					Instruction = instruction.Instruction
				});
			}
		}
	}
}
