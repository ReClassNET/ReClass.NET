namespace ReClassNET.Gui
{
	partial class SettingsDialog : IconForm
	{
		public SettingsDialog(Settings settings)
		{
			InitializeComponent();

			propertyGrid.SelectedObject = settings;
		}
	}
}
