using System;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET
{
	static class Program
	{
		private static Settings settings;
		private static ILogger logger;
		private static Random random = new Random();
		private static bool designMode = true;

		public static Settings Settings => settings;

		public static ILogger Logger => logger;

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
			logger = new GuiLogger();
#if RELEASE
			try
			{
				using (var nativeHelper = new NativeHelper())
				{
					var form = new MainForm(nativeHelper);

					Application.Run(form);
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
#else
			using (var nativeHelper = new NativeHelper())
			{
				var form = new MainForm(nativeHelper);

				Application.Run(form);
			}
#endif

			Settings.Save(settings, Constants.SettingsFile);
		}

		/// <summary>Shows the exception in a special form.</summary>
		/// <param name="ex">The exception.</param>
		public static void ShowException(Exception ex)
		{
			ex.HelpLink = Constants.HelpUrl;

			var msg = new ExceptionMessageBox(ex);
			msg.ShowToolBar = true;
			msg.Symbol = ExceptionMessageBoxSymbol.Error;
			msg.Show(null);
		}
	}
}
