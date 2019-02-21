namespace ReClassNET.UI
{
	interface ISettingsBindable
	{
		string SettingName { get; set; }

		object Source { get; set; }
	}
}
