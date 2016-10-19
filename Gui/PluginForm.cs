using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Plugins;

namespace ReClassNET.Gui
{
	partial class PluginForm : IconForm
	{
		private readonly PluginManager pluginManager;

		class PluginInfoRow
		{
			private readonly PluginInfo plugin;

			public Image Icon => plugin.Interface?.Icon ?? Properties.Resources.plugin;
			public string Name => plugin.Name;
			public string Version => plugin.FileVersion;
			public string Author => plugin.Author;
			public string Description => plugin.Description;

			public PluginInfoRow(PluginInfo plugin)
			{
				Contract.Requires(plugin != null);

				this.plugin = plugin;
			}
		}

		public PluginForm(PluginManager pluginManager)
		{
			Contract.Requires(pluginManager != null);

			InitializeComponent();

			this.pluginManager = pluginManager;

			pluginsDataGridView.AutoGenerateColumns = false;
			pluginsDataGridView.DataSource = pluginManager.Select(p => new PluginInfoRow(p)).ToList();

			UpdatePluginDescription();
		}

		private void pluginsDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			UpdatePluginDescription();
		}

		private void UpdatePluginDescription()
		{
			var row = pluginsDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
			if (row == null)
			{
				descriptionGroupBox.Text = string.Empty;
				descriptionLabel.Text = string.Empty;

				return;
			}

			var plugin = row.DataBoundItem as PluginInfoRow;
			if (plugin != null)
			{
				descriptionGroupBox.Text = plugin.Name;
				descriptionLabel.Text = plugin.Description;
			}
		}
	}
}
