namespace ReClassNET
{
	class Constants
	{
#if WIN64
		public const string Platform = "x64";
#else
		public const string Platform = "x86";
#endif

		public const string SettingsFile = "settings.xml";

		public const string PluginsFolder = "Plugins";
	}
}
