namespace ReClassNET
{
	public class Constants
	{
		public const string ApplicationName = "ReClass.NET";

		public const string ApplicationExecutableName = ApplicationName + ".exe";

		public const string ApplicationVersion = "1.2";

		public const string LauncherExecutableName = ApplicationName + "_Launcher.exe";

		public const string Author = "KN4CK3R";

		public const string HomepageUrl = "https://github.com/ReClassNET/ReClass.NET";

		public const string HelpUrl = "https://github.com/ReClassNET/ReClass.NET/issues";

		public const string PluginUrl = "https://github.com/ReClassNET/ReClass.NET#plugins";

#if RECLASSNET64
		public const string Platform = "x64";

		public const string AddressHexFormat = "X016";
#else
		public const string Platform = "x86";

		public const string AddressHexFormat = "X08";
#endif

		public const string SettingsFile = "settings.xml";

		public const string PluginsFolder = "Plugins";

		public static class CommandLineOptions
		{
			public const string AttachTo = "attachto";

			public const string FileExtRegister = "registerfileext";
			public const string FileExtUnregister = "unregisterfileext";
		}

		/// <summary>
		/// Change type for commandified members in classes which is used to signal what change occurred exactly. As we don't use this feature of the commandified
		/// class, this enum is defined to simply signal 'no specific change other than it changed' happened. 
		/// </summary>
		public enum GeneralPurposeChangeType
		{
			None
		}
	}
}
