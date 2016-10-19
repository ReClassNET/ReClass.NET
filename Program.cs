using System;
using System.Globalization;
using System.Windows.Forms;

namespace ReClassNET
{
	static class Program
	{
		private const string SettingsFile = "settings.xml";

		private static bool designMode = true;

		public static bool DesignMode => designMode;

		[STAThread]
		static void Main()
		{
			designMode = false; // The designer doesn't call Main()

			DpiUtil.ConfigureProcess();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

#if RELEASE
			try
			{
#endif
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

				var settings = Settings.Load(SettingsFile);

				using (var nativeHelper = new NativeHelper())
				{
					Application.Run(new MainForm(nativeHelper, settings));
				}

				Settings.Save(settings, SettingsFile);
#if RELEASE
			}
			catch (Exception ex)
			{
				ex.ShowDialog();
			}
#endif
		}
	}
}
