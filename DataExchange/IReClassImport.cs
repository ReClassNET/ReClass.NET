using ReClassNET.Logger;

namespace ReClassNET.DataExchange
{
	interface IReClassImport
	{
		SchemaBuilder Load(string filePath, ILogger logger);
	}
}
