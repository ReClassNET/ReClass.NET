using System.Diagnostics.Contracts;
using System.IO;
using ReClassNET.Logger;

namespace ReClassNET.DataExchange.ReClass
{
	[ContractClass(typeof(ReClassExportContract))]
	public interface IReClassExport
	{
		void Save(string filePath, ILogger logger);

		void Save(Stream output, ILogger logger);
	}

	[ContractClassFor(typeof(IReClassExport))]
	internal abstract class ReClassExportContract : IReClassExport
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
