using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;
using ReClassNET.Core;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.UI;

namespace ReClassNET
{
	static class Program
	{
		public static Settings Settings { get; private set; }

		public static ILogger Logger { get; private set; }

		public static Random GlobalRandom { get; } = new Random();

		public static RemoteProcess RemoteProcess { get; private set; }

		public static CoreFunctionsManager CoreFunctions => RemoteProcess.CoreFunctions;

		public static MainForm MainForm { get; private set; }

		public static bool DesignMode { get; private set; } = true;

		public static FontEx MonoSpaceFont { get; private set; }

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

			MonoSpaceFont = new FontEx
			{
				Font = new Font("Courier New", DpiUtil.ScaleIntX(13), GraphicsUnit.Pixel),
				Width = DpiUtil.ScaleIntX(8),
				Height = DpiUtil.ScaleIntY(16)
			};

			NativeMethods.EnableDebugPrivileges();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

			Settings = Settings.Load(Constants.SettingsFile);
			Logger = new GuiLogger();
#if DEBUG
			using (var coreFunctions = new CoreFunctionsManager())
			{
				RemoteProcess = new RemoteProcess(coreFunctions);

				MainForm = new MainForm();

				Application.Run(MainForm);

				RemoteProcess.Dispose();
			}
#else
			try
			{
				using (var coreFunctions = new CoreFunctionsManager())
				{
					RemoteProcess = new RemoteProcess(coreFunctions);

					MainForm = new MainForm();

					Application.Run(MainForm);

					RemoteProcess.Dispose();
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
