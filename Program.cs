using System;
using System.Globalization;
using System.Windows.Forms;

namespace ReClassNET
{
	static class Program
	{
		private const string SettingsFile = "settings.xml";

		[STAThread]
		static void Main()
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

			var settings = Settings.Load(SettingsFile);

			using (var nativeHelper = new NativeHelper())
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm(nativeHelper, settings));
			}

			Settings.Save(settings, SettingsFile);
		}
	}
}
