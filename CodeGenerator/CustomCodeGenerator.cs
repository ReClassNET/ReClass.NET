using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Nodes;
using ColorCode;

namespace ReClassNET.CodeGenerator
{
	[ContractClass(typeof(ICustomCodeGeneratorContract))]
	public interface ICustomCodeGenerator
	{
		bool CanGenerateCode(BaseNode node);

		MemberDefinition GetMemberDefinition(BaseNode node, Language language);
	}

	[ContractClassFor(typeof(ICustomCodeGenerator))]
	internal abstract class ICustomCodeGeneratorContract : ICustomCodeGenerator
	{
		public bool CanGenerateCode(BaseNode node)
		{
			Contract.Requires(node != null);

			throw new NotImplementedException();
		}

		public MemberDefinition GetMemberDefinition(BaseNode node, Language language)
		{
			Contract.Requires(node != null);
			Contract.Ensures(Contract.Result<MemberDefinition>() != null);

			throw new NotImplementedException();
		}
	}

	public class CustomCodeGenerator
	{
		private static readonly List<ICustomCodeGenerator> generators = new List<ICustomCodeGenerator>();

		public static void RegisterCustomType(ICustomCodeGenerator generator)
		{
			Contract.Requires(generator != null);

			generators.Add(generator);
		}

		public static void UnregisterCustomType(ICustomCodeGenerator generator)
		{
			Contract.Requires(generator != null);

			generators.Remove(generator);
		}

		public static ICustomCodeGenerator GetGenerator(BaseNode node)
		{
			Contract.Requires(node != null);

			return generators.Where(c => c.CanGenerateCode(node)).FirstOrDefault();
		}
	}

	class WeakPtrCodeGenerator : ICustomCodeGenerator
	{
		abstract class WeakPtrNode : BaseNode { }

		public bool CanGenerateCode(BaseNode node) => node is WeakPtrNode;

		public MemberDefinition GetMemberDefinition(BaseNode node, Language language)
		{
			if (language == Language.Cpp)

			throw new NotImplementedException();
		}
	}
}
