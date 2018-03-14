using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ReClassNET.Debugger;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class FoundCodeForm : IconForm
	{
		private class FoundCodeInfo
		{
			public ExceptionDebugInfo DebugInfo { get; set; }
			public DisassembledInstruction[] Instructions { get; set; }
		}

		public delegate void StopEventHandler(object sender, EventArgs e);

		private readonly RemoteProcess process;
		
		private readonly DataTable data;
		private volatile bool acceptNewRecords = true;

		public event StopEventHandler Stop;

		public FoundCodeForm(RemoteProcess process, IntPtr address, HardwareBreakpointTrigger trigger)
		{
			Contract.Requires(process != null);

			this.process = process;

			InitializeComponent();

			foundCodeDataGridView.AutoGenerateColumns = false;
			infoTextBox.Font = new Font(FontFamily.GenericMonospace, infoTextBox.Font.Size);

			if (trigger == HardwareBreakpointTrigger.Write)
			{
				Text = "Find out what writes to " + address.ToString(Constants.AddressHexFormat);
			}
			else
			{
				Text = "Find out what accesses " + address.ToString(Constants.AddressHexFormat);
			}

			bannerBox.Text = Text;

			data = new DataTable();
			data.Columns.Add("counter", typeof(int));
			data.Columns.Add("instruction", typeof(string));
			data.Columns.Add("info", typeof(FoundCodeInfo));

			foundCodeDataGridView.DataSource = data;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			GlobalWindowManager.RemoveWindow(this);
		}

		#region Event Handler

		private void foundCodeDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			var info = GetSelectedInfo();
			if (info == null)
			{
				return;
			}

			var sb = new StringBuilder();

			for (var i = 0; i < 5; ++i)
			{
				var code = $"{info.Instructions[i].Address.ToString(Constants.AddressHexFormat)} - {info.Instructions[i].Instruction}";
				if (i == 2)
				{
					sb.AppendLine(code + " <<<");
				}
				else
				{
					sb.AppendLine(code);
				}
			}

			sb.AppendLine();

#if RECLASSNET64
			sb.AppendLine($"RAX = {info.DebugInfo.Registers.Rax.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"RBX = {info.DebugInfo.Registers.Rbx.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"RCX = {info.DebugInfo.Registers.Rcx.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"RDX = {info.DebugInfo.Registers.Rdx.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"RDI = {info.DebugInfo.Registers.Rdi.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"RSI = {info.DebugInfo.Registers.Rsi.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"RSP = {info.DebugInfo.Registers.Rsp.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"RBP = {info.DebugInfo.Registers.Rbp.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"RIP = {info.DebugInfo.Registers.Rip.ToString(Constants.AddressHexFormat)}");

			sb.AppendLine($"R8  = {info.DebugInfo.Registers.R8.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"R9  = {info.DebugInfo.Registers.R9.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"R10 = {info.DebugInfo.Registers.R10.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"R11 = {info.DebugInfo.Registers.R11.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"R12 = {info.DebugInfo.Registers.R12.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"R13 = {info.DebugInfo.Registers.R13.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"R14 = {info.DebugInfo.Registers.R14.ToString(Constants.AddressHexFormat)}");
			sb.Append($"R15 = {info.DebugInfo.Registers.R15.ToString(Constants.AddressHexFormat)}");
#else
			sb.AppendLine($"EAX = {info.DebugInfo.Registers.Eax.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"EBX = {info.DebugInfo.Registers.Ebx.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"ECX = {info.DebugInfo.Registers.Ecx.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"EDX = {info.DebugInfo.Registers.Edx.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"EDI = {info.DebugInfo.Registers.Edi.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"ESI = {info.DebugInfo.Registers.Esi.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"ESP = {info.DebugInfo.Registers.Esp.ToString(Constants.AddressHexFormat)}");
			sb.AppendLine($"EBP = {info.DebugInfo.Registers.Ebp.ToString(Constants.AddressHexFormat)}");
			sb.Append($"EIP = {info.DebugInfo.Registers.Eip.ToString(Constants.AddressHexFormat)}");
#endif

			infoTextBox.Text = sb.ToString();
		}

		private void FoundCodeForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			StopRecording();
		}

		private void createFunctionButton_Click(object sender, EventArgs e)
		{
			var info = GetSelectedInfo();
			if (info == null)
			{
				return;
			}

			var disassembler = new Disassembler(process.CoreFunctions);
			var functionStartAddress = disassembler.RemoteGetFunctionStartAddress(process, info.DebugInfo.ExceptionAddress);
			if (functionStartAddress.IsNull())
			{
				MessageBox.Show("Could not find the start of the function. Aborting.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return;
			}

			var node = LinkedWindowFeatures.CreateClassAtAddress(functionStartAddress, false);
			node.AddNode(new FunctionNode
			{
				Comment = info.Instructions[2].Instruction
			});
		}

		private void stopButton_Click(object sender, EventArgs e)
		{
			StopRecording();

			stopButton.Visible = false;
			closeButton.Visible = true;
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		#endregion

		private FoundCodeInfo GetSelectedInfo()
		{
			var row = foundCodeDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
			var view = row?.DataBoundItem as DataRowView;
			return view?["info"] as FoundCodeInfo;
		}

		private void StopRecording()
		{
			acceptNewRecords = false;

			Stop?.Invoke(this, EventArgs.Empty);
		}

		public void AddRecord(ExceptionDebugInfo? context)
		{
			if (context == null)
			{
				return;
			}
			if (!acceptNewRecords)
			{
				return;
			}

			if (InvokeRequired)
			{
				Invoke((MethodInvoker)(() => AddRecord(context)));

				return;
			}

			var row = data.AsEnumerable().FirstOrDefault(r => r.Field<FoundCodeInfo>("info").DebugInfo.ExceptionAddress == context.Value.ExceptionAddress);
			if (row != null)
			{
				row["counter"] = row.Field<int>("counter") + 1;
			}
			else
			{
				var disassembler = new Disassembler(process.CoreFunctions);

				var causedByInstruction = disassembler.RemoteGetPreviousInstruction(process, context.Value.ExceptionAddress);
				if (causedByInstruction == null)
				{
					return;
				}

				var instructions = new DisassembledInstruction[5];
				instructions[2] = causedByInstruction;
				instructions[1] = disassembler.RemoteGetPreviousInstruction(process, instructions[2].Address);
				instructions[0] = disassembler.RemoteGetPreviousInstruction(process, instructions[1].Address);

				var i = 3;
				foreach (var instruction in disassembler.RemoteDisassembleCode(process, context.Value.ExceptionAddress, 2 * Disassembler.MaximumInstructionLength, 2))
				{
					instructions[i++] = instruction;
				}

				row = data.NewRow();
				row["counter"] = 1;
				row["instruction"] = causedByInstruction.Instruction;
				row["info"] = new FoundCodeInfo
				{
					DebugInfo = context.Value,
					Instructions = instructions
				};
				data.Rows.Add(row);
			}
		}
	}
}
