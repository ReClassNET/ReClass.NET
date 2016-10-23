namespace ReClassNET.UI
{
	interface ISettingsBindable
	{
		string SettingName { get; set; }

		Settings Source { get; set; }
	}
}
