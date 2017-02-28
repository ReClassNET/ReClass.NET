namespace ReClassNET
{
	internal class Constants
	{
		public const string ApplicationName = "ReClass.NET";

		public const string ApplicationVersion = "1.1";

		public const string Author = "KN4CK3R";

		public const string HomepageUrl = "https://github.com/KN4CK3R/ReClass.NET";

		public const string HelpUrl = "https://github.com/KN4CK3R/ReClass.NET/issues";

		public const string PluginUrl = "https://github.com/KN4CK3R/ReClass.NET#plugins";

#if WIN64
		public const string Platform = "x64";

		public const string StringHexFormat = "X016";
#else
		public const string Platform = "x86";

		public const string StringHexFormat = "X08";
#endif

		public const string SettingsFile = "settings.xml";

		public const string PluginsFolder = "Plugins";
	}
}
