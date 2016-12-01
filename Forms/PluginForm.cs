using System;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Memory;
using ReClassNET.Plugins;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class PluginForm : IconForm
	{
		private readonly PluginManager pluginManager;
		private readonly NativeHelper nativeHelper;

		class PluginInfoRow
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

		internal PluginForm(PluginManager pluginManager, NativeHelper nativeHelper)
		{
			Contract.Requires(pluginManager != null);
			Contract.Requires(nativeHelper != null);

			this.pluginManager = pluginManager;
			this.nativeHelper = nativeHelper;

			InitializeComponent();

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

			setAllComboBox.DisplayMember = nameof(NativeHelper.MethodInfo.Provider);
			setAllComboBox.DataSource = nativeHelper.MethodRegistry.Values
				.SelectMany(l => l)
				.Select(m => m.Provider)
				.Distinct()
				.ToList();
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

		private void NativeMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
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

		private void setAllComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			var provider = setAllComboBox.SelectedItem as string;
			if (provider == null)
			{
				return;
			}

			foreach (var cb in new[]
			{
				enumerateProcessesComboBox,
				enumerateRemoteSectionsAndModulesComboBox,
				isProcessValidComboBox,
				openRemoteProcessComboBox,
				closeRemoteProcessComboBox,
				readRemoteMemoryComboBox,
				writeRemoteMemoryComboBox,
				disassembleRemoteCodeComboBox,
				controlRemoteProcessComboBox
			})
			{
				var method = cb.Items.OfType<NativeHelper.MethodInfo>().Where(m => m.Provider == provider).FirstOrDefault();
				if (method != null)
				{
					if (cb.SelectedItem != method)
					{
						cb.SelectedItem = method;
						nativeHelper.SetActiveNativeMethod(method);
					}
				}
			}
		}

		private void getMoreLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Constants.PluginUrl);
		}

		#endregion

		private void FillComboBox(ComboBox cb, NativeHelper.RequestFunction method)
		{
			Contract.Requires(cb != null);

			var methods = nativeHelper.MethodRegistry[method];

			var selectedFnPtr = nativeHelper.RequestFunctionPtr(method);

			cb.DisplayMember = nameof(NativeHelper.MethodInfo.Provider);
			cb.DataSource = methods;
			cb.SelectedIndex = methods.FindIndex(m => m.FunctionPtr == selectedFnPtr);
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
