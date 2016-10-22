using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ReClassNET.Forms;

namespace ReClassNET.Logger
{
	class GuiLogger : BaseLogger
	{
		private LogForm form;

		public LogLevel Level { get; set; } = LogLevel.Warning;

		public GuiLogger()
		{
			form = new LogForm();
			form.FormClosing += delegate (object sender, FormClosingEventArgs e)
			{
				form.Clear();

				form.Hide();

				e.Cancel = true;
			};

			NewLogEntry += OnNewLogEntry;
		}

		private void OnNewLogEntry(LogLevel level, string message)
		{
			Contract.Requires(message != null);

			if (level < Level)
			{
				return;
			}

			ShowForm();

			form.Add(level, message);
		}

		public void ShowForm()
		{
			if (!form.Visible)
			{
				form.Show();
			}
		}
	}
}
