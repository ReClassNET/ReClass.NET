namespace ReClassNET.DataExchange
{
	delegate void ReportError(string message);

	interface IReClassImport
	{
		SchemaBuilder Load(string filePath, ReportError report);
	}
}
