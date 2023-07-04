using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;
using ReClassNET.Core;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.UI;
using ReClassNET.Util;
using SD.Tools.Algorithmia.Commands;

namespace ReClassNET
{
	public static class Program
	{
		public static CommandLineArgs CommandLineArgs { get; private set; }

		public static Settings Settings { get; private set; }

		public static ILogger Logger { get; private set; }

		public static Random GlobalRandom { get; } = new Random();

		public static RemoteProcess RemoteProcess { get; private set; }

		public static CoreFunctionsManager CoreFunctions => RemoteProcess.CoreFunctions;

		public static MainForm MainForm { get; private set; }

		public static bool DesignMode { get; private set; } = true;

		public static FontEx MonoSpaceFont { get; private set; }

		public static Guid CommandQueueID { get; private set; }

		[STAThread]
		static void Main(string[] args)
		{
			DesignMode = false; // The designer doesn't call Main()
			CommandQueueID = Guid.NewGuid();

			// wire event handlers for unhandled exceptions, so these will be shown using our own method.
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic, true);
			Application.ThreadException += new ThreadExceptionEventHandler(Program.Application_ThreadException);
			AppDomain.CurrentDomain.UnhandledException+=new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			CommandLineArgs = new CommandLineArgs(args);

			try
			{
				DpiUtil.ConfigureProcess();
				DpiUtil.TrySetDpiFromCurrentDesktop();
			}
			catch
			{
				// ignored
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

			// switch is set to false, so Do actions during Undo actions are ignored.
			CommandQueueManager.ThrowExceptionOnDoDuringUndo = false;
			// activate our command queue stack. We're only changing things from the main thread so we don't need multiple stacks.
			CommandQueueManagerSingleton.GetInstance().ActivateCommandQueueStack(CommandQueueID);

			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

			Settings = SettingsSerializer.Load();
			Logger = new GuiLogger();

			if (!NativeMethods.IsUnix() && Settings.RunAsAdmin && !WinUtil.IsAdministrator)
			{
				WinUtil.RunElevated(Process.GetCurrentProcess().MainModule?.FileName, args.Length > 0 ? string.Join(" ", args) : null);
				return;
			}

#if !DEBUG
			try
			{
#endif
			using (var coreFunctions = new CoreFunctionsManager())
			{
				RemoteProcess = new RemoteProcess(coreFunctions);

				MainForm = new MainForm();

				Application.Run(MainForm);

				RemoteProcess.Dispose();
			}
#if !DEBUG
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
#endif

			SettingsSerializer.Save(Settings);
		}
		
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			ShowException(e.Exception);
		}
		
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ShowException(e.ExceptionObject as Exception);
		}
		
		/// <summary>Shows the exception in a special form.</summary>
		/// <param name="ex">The exception.</param>
		public static void ShowException(Exception ex)
		{
			ex.HelpLink = Constants.HelpUrl;

			var msg = new ExceptionMessageBox(ex)
			{
				Beep = false,
				ShowToolBar = true,
				Symbol = ExceptionMessageBoxSymbol.Error
			};
			msg.Show(null);
		}
	}
}
