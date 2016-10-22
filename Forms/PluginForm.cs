using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Plugins;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	partial class PluginForm : IconForm
	{
		private readonly PluginManager pluginManager;
		private readonly NativeHelper nativeHelper;

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

		public PluginForm(PluginManager pluginManager, NativeHelper nativeHelper)
		{
			Contract.Requires(pluginManager != null);
			Contract.Requires(nativeHelper != null);

			InitializeComponent();

			this.pluginManager = pluginManager;
			this.nativeHelper = nativeHelper;

			// Plugins Tab

			pluginsDataGridView.AutoGenerateColumns = false;
			pluginsDataGridView.DataSource = pluginManager.Select(p => new PluginInfoRow(p)).ToList();

			UpdatePluginDescription();

			// Native Methods Tab

			FillComboBox(enumerateProcessesComboBox, NativeHelper.RequestFunction.EnumerateProcesses);
			FillComboBox(enumerateRemoteSectionsAndModulesComboBox, NativeHelper.RequestFunction.EnumerateRemoteSectionsAndModules);
			FillComboBox(isProcessValidComboBox, NativeHelper.RequestFunction.IsProcessValid);
			FillComboBox(openRemoteProcessComboBox, NativeHelper.RequestFunction.OpenRemoteProcess);
			FillComboBox(closeRemoteProcessComboBox, NativeHelper.RequestFunction.CloseRemoteProcess);
			FillComboBox(readRemoteMemoryComboBox, NativeHelper.RequestFunction.ReadRemoteMemory);
			FillComboBox(writeRemoteMemoryComboBox, NativeHelper.RequestFunction.WriteRemoteMemory);
			FillComboBox(disassembleRemoteCodeComboBox, NativeHelper.RequestFunction.DisassembleRemoteCode);
			FillComboBox(controlRemoteProcessComboBox, NativeHelper.RequestFunction.ControlRemoteProcess);
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

		private void FillComboBox(ComboBox cb, NativeHelper.RequestFunction method)
		{
			var methods = nativeHelper.MethodRegistry[method];

			cb.DisplayMember = nameof(NativeHelper.MethodInfo.Provider);
			cb.DataSource = methods;
			cb.SelectedIndex = methods.FindIndex(m => m.FunctionPtr == nativeHelper.RequestFunctionPtr(method));
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

		private void NativeMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var cb = sender as ComboBox;
			if (cb == null)
			{
				return;
			}

			var methodInfo = cb.SelectedItem as NativeHelper.MethodInfo;
			if (methodInfo != null)
			{
				nativeHelper.SetActiveNativeMethod(methodInfo);
			}
		}
	}
}
