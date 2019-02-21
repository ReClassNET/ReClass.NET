namespace ReClassNET.UI
{
	interface IPropertyBindable
	{
		string PropertyName { get; set; }

		object Source { get; set; }
	}
}
