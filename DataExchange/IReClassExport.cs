using System.Diagnostics.Contracts;
using System.IO;
using ReClassNET.Logger;

namespace ReClassNET.DataExchange
{
	[ContractClass(typeof(IReClassExportContract))]
	public interface IReClassExport
	{
		void Save(string filePath, ILogger logger);

		void Save(Stream output, ILogger logger);
	}

	[ContractClassFor(typeof(IReClassExport))]
	internal abstract class IReClassExportContract : IReClassExport
	{
		public void Save(string filePath, ILogger logger)
		{
			Contract.Requires(filePath != null);
			Contract.Requires(logger != null);
		}

		public void Save(Stream output, ILogger logger)
		{
			Contract.Requires(output != null);
			Contract.Requires(logger != null);
		}
	}
}
