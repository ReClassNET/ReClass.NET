using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Logger;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	partial class LogForm : IconForm
	{
		private class LogItem
		{
			public Image Icon { get; set; }

			public string Message { get; set; }
		}

		private readonly List<LogItem> items = new List<LogItem>();

		public LogForm()
		{
			InitializeComponent();

			entriesDataGridView.AutoGenerateColumns = false;
			entriesDataGridView.DataSource = items;
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

		public void Clear()
		{
			items.Clear();

			RefreshDataBinding();
		}

		public void Add(LogLevel level, string message)
		{
			Contract.Requires(message != null);

			Image icon;
			switch (level)
			{
				case LogLevel.Error:
					icon = Properties.Resources.error;
					break;
				case LogLevel.Warning:
					icon = Properties.Resources.warning;
					break;
				case LogLevel.Information:
					icon = Properties.Resources.information;
					break;
				default:
					icon = Properties.Resources.cog;
					break;
			}

			items.Add(new LogItem { Icon = icon, Message = message });

			RefreshDataBinding();
		}

		private void RefreshDataBinding()
		{
			var cm = entriesDataGridView.BindingContext[items] as CurrencyManager;
			if (cm != null)
			{
				cm.Refresh();
			}
		}

		private void copyToClipboardButton_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(items.Select(i => i.Message).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}"));
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
