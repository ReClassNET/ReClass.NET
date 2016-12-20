using System;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;
using ReClassNET.Core;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Native;
using ReClassNET.UI;

namespace ReClassNET
{
	static class Program
	{
		private static Settings settings;
		private static ILogger logger;
		private static Random random = new Random();
		private static MainForm mainForm;
		private static bool designMode = true;

		public static Settings Settings => settings;

		public static ILogger Logger => logger;

		public static Random GlobalRandom => random;

		public static MainForm MainForm => mainForm;

		public static bool DesignMode => designMode;

		[STAThread]
		static void Main()
		{
			designMode = false; // The designer doesn't call Main()

			DpiUtil.ConfigureProcess();

			NativeMethods.EnableDebugPrivileges();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

			settings = Settings.Load(Constants.SettingsFile);
			logger = new GuiLogger();
#if DEBUG
			using (var coreFunctions = new CoreFunctionsManager())
			{
				mainForm = new MainForm(coreFunctions);

				Application.Run(mainForm);
			}
#else
			try
			{
				using (var nativeHelper = new CoreFunctionsManager())
				{
					mainForm = new MainForm(nativeHelper);

					Application.Run(mainForm);
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
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
