using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Plugins;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class PluginForm : IconForm
	{
		private class PluginInfoRow
		{
			private readonly PluginInfo plugin;

			public Image Icon => plugin.Interface?.Icon ?? Properties.Resources.B16x16_Plugin;
			public string Name => plugin.Name;
			public string Version => plugin.FileVersion;
			public string Author => plugin.Author;
			public string Description => plugin.Description;

			public PluginInfoRow(PluginInfo plugin)
			{
				Contract.Requires(plugin != null);
				Contract.Ensures(this.plugin != null);

				this.plugin = plugin;
			}
		}

		internal PluginForm(PluginManager pluginManager)
		{
			Contract.Requires(pluginManager != null);

			InitializeComponent();

			// Plugins Tab

			pluginsDataGridView.AutoGenerateColumns = false;
			pluginsDataGridView.DataSource = pluginManager.Plugins.Select(p => new PluginInfoRow(p)).ToList();

			UpdatePluginDescription();

			// Native Methods Tab

			var providers = Program.CoreFunctions.FunctionProviders.ToArray();
			functionsProvidersComboBox.Items.AddRange(providers);
			functionsProvidersComboBox.SelectedIndex = Array.IndexOf(providers, Program.CoreFunctions.CurrentFunctionsProvider);
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

		private void pluginsDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			UpdatePluginDescription();
		}

		private void functionsProvidersComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (!(functionsProvidersComboBox.SelectedItem is string provider))
			{
				return;
			}

			Program.CoreFunctions.SetActiveFunctionsProvider(provider);
		}

		private void getMoreLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Constants.PluginUrl);
		}

		#endregion

		private void UpdatePluginDescription()
		{
			var row = pluginsDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
			if (row == null)
			{
				descriptionGroupBox.Text = string.Empty;
				descriptionLabel.Text = string.Empty;

				return;
			}

			if (row.DataBoundItem is PluginInfoRow plugin)
			{
				descriptionGroupBox.Text = plugin.Name;
				descriptionLabel.Text = plugin.Description;
			}
		}
	}
}
