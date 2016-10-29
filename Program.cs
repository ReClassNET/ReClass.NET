using System;
using System.Globalization;
using System.Windows.Forms;
using ReClassNET.Forms;
using ReClassNET.UI;
using ReClassNET.Util;

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

			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

			settings = Settings.Load(Constants.SettingsFile);

#if RELEASE
			try
			{
				using (var nativeHelper = new NativeHelper())
				{
					var form = new MainForm(nativeHelper, settings);

					Application.Run(form);
				}
			}
			catch (Exception ex)
			{
				ex.ShowDialog();
			}
#else
			using (var nativeHelper = new NativeHelper())
			{
				var form = new MainForm(nativeHelper, settings);

				Application.Run(form);
			}
#endif

			Settings.Save(settings, Constants.SettingsFile);
		}
	}
}
