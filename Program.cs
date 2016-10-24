using ReClassNET.Forms;
using System;
using System.Globalization;
using System.Windows.Forms;
using ReClassNET.Util;
using ReClassNET.UI;

namespace ReClassNET
{
	static class Program
	{
		private static Settings settings;
		private static Random random = new Random();
		private static bool designMode = true;

		public static Settings Settings => settings;

		public static Random GlobalRandom => random;

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

				settings = Settings.Load(Constants.SettingsFile);

				using (var nativeHelper = new NativeHelper())
				{
					var form = new MainForm(nativeHelper, settings);

					Application.Run(form);
				}

				Settings.Save(settings, Constants.SettingsFile);
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
