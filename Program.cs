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
		public static Settings Settings { get; private set; }

		public static ILogger Logger { get; private set; }

		public static Random GlobalRandom { get; } = new Random();

		public static MainForm MainForm { get; private set; }

		public static bool DesignMode { get; private set; } = true;

		[STAThread]
		static void Main()
		{
			DesignMode = false; // The designer doesn't call Main()

			try
			{
				DpiUtil.ConfigureProcess();
			}
			catch
			{
				
			}

			NativeMethods.EnableDebugPrivileges();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

			Settings = Settings.Load(Constants.SettingsFile);
			Logger = new GuiLogger();
#if DEBUG
			using (var coreFunctions = new CoreFunctionsManager())
			{
				MainForm = new MainForm(coreFunctions);

				Application.Run(MainForm);
			}
#else
			try
			{
				using (var nativeHelper = new CoreFunctionsManager())
				{
					MainForm = new MainForm(nativeHelper);

					Application.Run(MainForm);
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
#endif

			Settings.Save(Settings, Constants.SettingsFile);
		}

		/// <summary>Shows the exception in a special form.</summary>
		/// <param name="ex">The exception.</param>
		public static void ShowException(Exception ex)
		{
			ex.HelpLink = Constants.HelpUrl;

			var msg = new ExceptionMessageBox(ex)
			{
				ShowToolBar = true,
				Symbol = ExceptionMessageBoxSymbol.Error
			};
			msg.Show(null);
		}
	}
}
