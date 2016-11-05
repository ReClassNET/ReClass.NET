using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ReClassNET.Forms;
using System;

namespace ReClassNET.Logger
{
	/// <summary>A logger which displays messages in a form.</summary>
	class GuiLogger : BaseLogger
	{
		private readonly LogForm form;

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

		private void OnNewLogEntry(LogLevel level, string message, Exception ex)
		{
			Contract.Requires(message != null);

			if (level < Level)
			{
				return;
			}

			ShowForm();

			form.Add(level, message, ex);
		}

		public void ShowForm()
		{
			if (!form.Visible)
			{
				form.Show();

				form.BringToFront();
			}
		}
	}
}
