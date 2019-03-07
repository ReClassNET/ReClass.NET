﻿using System;
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
using ReClassNET.Util;

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

		[STAThread]
		static void Main(string[] args)
		{
			DesignMode = false; // The designer doesn't call Main()

			CommandLineArgs = new CommandLineArgs(args);

			try
			{
				DpiUtil.ConfigureProcess();
			}
			catch
			{
				
			}

			MonoSpaceFont = new FontEx
			{
				Font = new Font("Courier-New", DpiUtil.ScaleIntX(13), GraphicsUnit.Pixel),
				Width = DpiUtil.ScaleIntX(8),
				Height = DpiUtil.ScaleIntY(16)
			};

			NativeMethods.EnableDebugPrivileges();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

			Settings = SettingsSerializer.Load();
			Logger = new GuiLogger();

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
