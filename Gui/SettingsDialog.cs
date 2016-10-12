using System.Windows.Forms;

namespace ReClassNET.Gui
{
	public partial class SettingsDialog : Form
	{
		public SettingsDialog(Settings settings)
		{
			InitializeComponent();

			propertyGrid.SelectedObject = settings;
		}
	}
}
