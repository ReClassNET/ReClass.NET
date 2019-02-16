using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	[ContractClass(typeof(CustomCodeGeneratorContract))]
	public interface ICustomCodeGenerator
	{
		bool CanGenerateCode(BaseNode node, Language language);

		MemberDefinition GetMemberDefinition(BaseNode node, Language language, ILogger logger);
	}

	[ContractClassFor(typeof(ICustomCodeGenerator))]
	internal abstract class CustomCodeGeneratorContract : ICustomCodeGenerator
	{
		public bool CanGenerateCode(BaseNode node, Language language)
		{
			Contract.Requires(node != null);

			throw new NotImplementedException();
		}

		public MemberDefinition GetMemberDefinition(BaseNode node, Language language, ILogger logger)
		{
			Contract.Requires(node != null);
			Contract.Ensures(Contract.Result<MemberDefinition>() != null);

			throw new NotImplementedException();
		}
	}

	internal class CustomCodeGenerator
	{
		private static readonly ISet<ICustomCodeGenerator> generators = new HashSet<ICustomCodeGenerator>();

		public static void Add(ICustomCodeGenerator generator)
		{
			Contract.Requires(generator != null);

			generators.Add(generator);
		}

		public static void Remove(ICustomCodeGenerator generator)
		{
			Contract.Requires(generator != null);

			generators.Remove(generator);
		}

		public static ICustomCodeGenerator GetGenerator(BaseNode node, Language language)
		{
			Contract.Requires(node != null);

			return generators.FirstOrDefault(c => c.CanGenerateCode(node, language));
		}
	}
}
