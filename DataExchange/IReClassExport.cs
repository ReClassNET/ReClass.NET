namespace ReClassNET.DataExchange
{
	interface IReClassExport
	{
		void Save(string filePath, SchemaBuilder schema);
	}
}
