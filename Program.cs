using System;
using System.Globalization;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Native;
using ReClassNET.UI;
using ReClassNET.Util;

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

			EnableDebugPrivileges();

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
					mainForm = new MainForm(nativeHelper);

					Application.Run(mainForm);
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
#else
			using (var nativeHelper = new NativeHelper())
			{
				mainForm = new MainForm(nativeHelper);

				Application.Run(mainForm);
			}
#endif

			Settings.Save(settings, Constants.SettingsFile);
		}

		private static bool EnableDebugPrivileges()
		{
			var result = false;

			IntPtr token;
			if (NativeMethods.OpenProcessToken(System.Diagnostics.Process.GetCurrentProcess().Handle, TokenAccessLevels.AllAccess, out token))
			{
				var privileges = new NativeMethods.TOKEN_PRIVILEGES();
				privileges.PrivilegeCount = 1;
				privileges.Luid.LowPart = 0x14;
				privileges.Luid.HighPart = 0;
				privileges.Attributes = 2;

				result = NativeMethods.AdjustTokenPrivileges(token, false, ref privileges, 0, IntPtr.Zero, IntPtr.Zero);

				NativeMethods.CloseHandle(token);
			}

			return result;
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
