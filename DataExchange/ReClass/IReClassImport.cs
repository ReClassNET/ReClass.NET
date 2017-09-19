using System;
using System.Diagnostics.Contracts;
using ReClassNET.Logger;

namespace ReClassNET.DataExchange.ReClass
{
	[ContractClass(typeof(ReClassImportContract))]
	public interface IReClassImport
	{
		void Load(string filePath, ILogger logger);
	}

	[ContractClassFor(typeof(IReClassImport))]
	internal abstract class ReClassImportContract : IReClassImport
	{
		public void Load(string filePath, ILogger logger)
		{
			Contract.Requires(filePath != null);
			Contract.Requires(logger != null);

			throw new NotImplementedException();
		}
	}
}
