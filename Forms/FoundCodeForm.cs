using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ReClassNET.Debugger;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class FoundCodeForm : IconForm
	{
		private class FoundCodeInfo
		{
			public ExceptionDebugInfo DebugInfo;
			public DisassembledInstruction[] Instructions;
		}

		public delegate void StopEventHandler(object sender, EventArgs e);

		private readonly RemoteProcess process;
		
		private DataTable data;

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
				Text = "Find out what writes to " + address.ToString(Constants.StringHexFormat);
			}
			else
			{
				Text = "Find out what accesses " + address.ToString(Constants.StringHexFormat);
			}

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

		#endregion

		public void AddRecord(ExceptionDebugInfo? context)
		{
			if (context == null)
			{
				return;
			}
			if (IsDisposed)
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
				var disassembler = new Disassembler(process.NativeHelper);
				var causedByInstruction = disassembler.RemoteGetPreviousInstruction(process, context.Value.ExceptionAddress);

				var instructions = new DisassembledInstruction[5];
				instructions[2] = causedByInstruction;
				instructions[1] = disassembler.RemoteGetPreviousInstruction(process, instructions[2].Address);
				instructions[0] = disassembler.RemoteGetPreviousInstruction(process, instructions[1].Address);

				var nextInstructions = disassembler.RemoteDisassembleCode(process, context.Value.ExceptionAddress, 30);
				for (var i = 0; i < 2; ++i)
				{
					instructions[3 + i] = nextInstructions[i];
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

		private void foundCodeDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			var row = foundCodeDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
			if (row == null)
			{
				infoTextBox.Text = string.Empty;

				return;
			}

			var view = row.DataBoundItem as DataRowView;
			if (view == null)
			{
				return;
			}

			var info = view["info"] as FoundCodeInfo;
			if (info == null)
			{
				return;
			}

			var sb = new StringBuilder();

			for (var i = 0; i < 5; ++i)
			{
				var code = $"{info.Instructions[i].Address.ToString(Constants.StringHexFormat)} - {info.Instructions[i].Instruction}";
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

#if WIN64
			sb.AppendLine($"RAX = {info.DebugInfo.Registers.Rax.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"RBX = {info.DebugInfo.Registers.Rbx.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"RCX = {info.DebugInfo.Registers.Rcx.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"RDX = {info.DebugInfo.Registers.Rdx.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"RDI = {info.DebugInfo.Registers.Rdi.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"RSI = {info.DebugInfo.Registers.Rsi.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"RSP = {info.DebugInfo.Registers.Rsp.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"RBP = {info.DebugInfo.Registers.Rbp.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"RIP = {info.DebugInfo.Registers.Rip.ToString(Constants.StringHexFormat)}");

			sb.AppendLine($"R8  = {info.DebugInfo.Registers.R8.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"R9  = {info.DebugInfo.Registers.R9.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"R10 = {info.DebugInfo.Registers.R10.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"R11 = {info.DebugInfo.Registers.R11.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"R12 = {info.DebugInfo.Registers.R12.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"R13 = {info.DebugInfo.Registers.R13.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"R14 = {info.DebugInfo.Registers.R14.ToString(Constants.StringHexFormat)}");
			sb.Append($"R15 = {info.DebugInfo.Registers.R15.ToString(Constants.StringHexFormat)}");
#else
			sb.AppendLine($"EAX = {info.DebugInfo.Registers.Eax.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"EBX = {info.DebugInfo.Registers.Ebx.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"ECX = {info.DebugInfo.Registers.Ecx.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"EDX = {info.DebugInfo.Registers.Edx.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"EDI = {info.DebugInfo.Registers.Edi.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"ESI = {info.DebugInfo.Registers.Esi.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"ESP = {info.DebugInfo.Registers.Esp.ToString(Constants.StringHexFormat)}");
			sb.AppendLine($"EBP = {info.DebugInfo.Registers.Ebp.ToString(Constants.StringHexFormat)}");
			sb.Append($"EIP = {info.DebugInfo.Registers.Eip.ToString(Constants.StringHexFormat)}");
#endif

			infoTextBox.Text = sb.ToString();
		}

		private void FoundCodeForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Stop?.Invoke(this, EventArgs.Empty);
		}

		private void stopButton_Click(object sender, EventArgs e)
		{
			Stop?.Invoke(this, EventArgs.Empty);

			stopButton.Visible = false;
			closeButton.Visible = true;
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
