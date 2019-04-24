using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;

namespace ReClassNET.Util
{
	public class PathUtil
	{
		private static readonly Lazy<string> executablePath = new Lazy<string>(() =>
		{
			string path = null;
			try
			{
				path = Assembly.GetExecutingAssembly().Location;
			}
			catch (Exception)
			{

			}

			if (string.IsNullOrEmpty(path))
			{
				path = Assembly.GetExecutingAssembly().GetName().CodeBase;
				path = FileUrlToPath(path);
			}

			return path;
		});

		/// <summary>Gets the full pathname of the executable file.</summary>
		public static string ExecutablePath => executablePath.Value;

		private static readonly Lazy<string> executableFolderPath = new Lazy<string>(() => Path.GetDirectoryName(executablePath.Value));

		/// <summary>Gets the full pathname of the executable folder.</summary>
		public static string ExecutableFolderPath => executableFolderPath.Value;

		private static readonly Lazy<string> temporaryFolderPath = new Lazy<string>(Path.GetTempPath);

		/// <summary>Gets the full pathname of the temporary folder.</summary>
		/// <remarks>%temp%</remarks>
		public static string TemporaryFolderPath => temporaryFolderPath.Value;

		private static readonly Lazy<string> settingsFolderPath = new Lazy<string>(() =>
		{
			string applicationData;
			try
			{
				applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			}
			catch (Exception)
			{
				applicationData = executableFolderPath.Value;
			}

			string localApplicationData;
			try
			{
				localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			}
			catch (Exception)
			{
				localApplicationData = applicationData;
			}

			return Path.Combine(localApplicationData, Constants.ApplicationName);
		});

		/// <summary>Gets the full pathname of the settings folder.</summary>
		/// <remarks>%localappdata%\ReClass.NET\</remarks>
		public static string SettingsFolderPath => settingsFolderPath.Value;

		private static readonly Lazy<string> launcherExecutablePath = new Lazy<string>(() =>
		{
			var path = Path.Combine(Directory.GetParent(ExecutableFolderPath).FullName, Constants.LauncherExecutableName);
			return !File.Exists(path) ? null : path;
		});

		/// <summary>Gets the full pathname of the launcher executable.</summary>
		public static string LauncherExecutablePath => launcherExecutablePath.Value;

		/// <summary>Converts a file url to a normal path.</summary>
		/// <param name="url">URL of the file.</param>
		/// <returns>The path part of the URL.</returns>
		public static string FileUrlToPath(string url)
		{
			Contract.Requires(url != null);

			if (url.StartsWith("file:///", StringComparison.OrdinalIgnoreCase))
			{
				url = url.Substring(8);
			}

			url = url.Replace('/', Path.DirectorySeparatorChar);

			return url;
		}
	}
}
