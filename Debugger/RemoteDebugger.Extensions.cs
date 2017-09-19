using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace ReClassNET.Debugger
{
	public static class RemoteDebuggerExtensions
	{
		public static bool AskUserAndAttachDebugger(this RemoteDebugger debugger)
		{
			Contract.Requires(debugger != null);

			return debugger.StartDebuggerIfNeeded(
				() => MessageBox.Show(
					"This will attach the debugger of ReClass.NET to the current process. Continue?",
					"Confirmation",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				) == DialogResult.Yes
			);
		}
	}
}
