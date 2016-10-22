using System;
using System.Windows.Forms;
using ReClassNET.UI;

namespace ReClassNET.Gui
{
	partial class SettingsForm : IconForm
	{
		public SettingsForm(Settings settings)
		{
			InitializeComponent();

			propertyGrid.SelectedObject = settings;
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
	}
}
