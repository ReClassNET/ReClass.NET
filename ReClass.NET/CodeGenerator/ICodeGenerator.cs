using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;

namespace ReClassNET.CodeGenerator
{
	[ContractClass(typeof(CodeGeneratorContract))]
	public interface ICodeGenerator
	{
		/// <summary>The language this generator produces.</summary>
		Language Language { get; }

		/// <summary>Generates code for the classes.</summary>
		/// <param name="classes">The classes to generate code from.</param>
		/// <param name="logger">The logger used to output messages.</param>
		/// <returns>The code for the classes.</returns>
		string GenerateCode(IReadOnlyList<ClassNode> classes, IReadOnlyList<EnumDescription> enums, ILogger logger);
	}

	[ContractClassFor(typeof(ICodeGenerator))]
	internal abstract class CodeGeneratorContract : ICodeGenerator
	{
		public Language Language => throw new NotImplementedException();

		public string GenerateCode(IReadOnlyList<ClassNode> classes, IReadOnlyList<EnumDescription> enums, ILogger logger)
		{
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes, c => c != null));
			Contract.Requires(logger != null);

			Contract.Ensures(Contract.Result<string>() != null);

			throw new NotImplementedException();
		}
	}
}
