using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ColorCode;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	[ContractClass(typeof(ICodeGeneratorContract))]
	public interface ICodeGenerator
	{
		ILanguage Language { get; }

		string GetCodeFromClasses(IEnumerable<ClassNode> classes);
	}

	[ContractClassFor(typeof(ICodeGenerator))]
	internal abstract class ICodeGeneratorContract : ICodeGenerator
	{
		public ILanguage Language
		{
			get
			{
				Contract.Ensures(Contract.Result<ILanguage>() != null);

				throw new NotImplementedException();
			}
		}

		public string GetCodeFromClasses(IEnumerable<ClassNode> classes)
		{
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes, c => c != null));

			Contract.Ensures(Contract.Result<string>() != null);

			throw new NotImplementedException();
		}
	}
}
