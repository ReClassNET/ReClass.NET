using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ReClassNET;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET_Launcher
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			var commandLineArgs = new CommandLineArgs(args);

			// Register the files with the launcher.
			if (commandLineArgs[Constants.CommandLineOptions.FileExtRegister] != null)
			{
				NativeMethods.RegisterExtension(ReClassNetFile.FileExtension, ReClassNetFile.FileExtensionId, PathUtil.ExecutablePath, Constants.ApplicationName);

				return;
			}
			if (commandLineArgs[Constants.CommandLineOptions.FileExtUnregister] != null)
			{
				NativeMethods.UnregisterExtension(ReClassNetFile.FileExtension, ReClassNetFile.FileExtensionId);

				return;
			}

			var is64Bit = IntPtr.Size == 8;

			// If there is a file in the commandline, read the platform.
			if (commandLineArgs.FileName != null)
			{
				try
				{
					is64Bit = ReClassNetFile.ReadPlatform(commandLineArgs.FileName) == "x64";
				}
				catch (Exception)
				{
					
				}
			}

			// And finally start the real ReClass.NET.
			var applicationPath = Path.Combine(PathUtil.ExecutableFolderPath, is64Bit ? "x64" : "x86", Constants.ApplicationExecutableName);

			try
			{
				var processStartInfo = new ProcessStartInfo
				{
					FileName = applicationPath,
					UseShellExecute = true,
					WindowStyle = ProcessWindowStyle.Normal
				};
				var arguments = GetCommandLineWithoutExecutablePath();
				if (arguments != null)
				{
					processStartInfo.Arguments = arguments;
				}

				Process.Start(processStartInfo);
			}
			catch (Exception)
			{
				MessageBox.Show($"Could not start '{applicationPath}'.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>Gets command line without the executable path.</summary>
		/// <returns>If empty <c>null</c> else the command line parameters.</returns>
		private static string GetCommandLineWithoutExecutablePath()
		{
			var commandLine = Environment.CommandLine;

			if (string.IsNullOrEmpty(commandLine))
			{
				return null;
			}

			var arguments = string.Empty;
			int argIndex;

			if (commandLine[0] == '"')
			{
				var secondDoublequoteIndex = -1;
				for (var i = 1; i < commandLine.Length; ++i)
				{
					if (commandLine[i] == '\\')
					{
						++i;
						continue;
					}
					if (commandLine[i] == '"')
					{
						secondDoublequoteIndex = i + 1;
						break;
					}
				}
				argIndex = secondDoublequoteIndex;
			}
			else
			{
				argIndex = commandLine.IndexOf(" ", StringComparison.Ordinal);
			}
			if (argIndex != -1)
			{
				arguments = commandLine.Substring(argIndex + 1);
			}

			return arguments == string.Empty ? null : arguments;
		}
	}
}
