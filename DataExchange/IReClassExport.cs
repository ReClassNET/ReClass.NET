using System.Diagnostics.Contracts;
using ReClassNET.Logger;

namespace ReClassNET.DataExchange
{
	[ContractClass(typeof(IReClassExportContract))]
	public interface IReClassExport
	{
		void Save(string filePath, SchemaBuilder schema, ILogger logger);
	}

	[ContractClassFor(typeof(IReClassExport))]
	internal abstract class IReClassExportContract : IReClassExport
	{
		public void Save(string filePath, SchemaBuilder schema, ILogger logger)
		{
			Contract.Requires(filePath != null);
			Contract.Requires(schema != null);
			Contract.Requires(logger != null);
		}
	}
}
