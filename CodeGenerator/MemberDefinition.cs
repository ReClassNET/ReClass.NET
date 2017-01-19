using System.Diagnostics.Contracts;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	public class MemberDefinition
	{
		public bool IsArray => ArrayCount != 0;
		public int ArrayCount { get; }

		public int Offset { get; }

		public string Type { get; }
		public string Name { get; }
		public string Comment { get; }

		public MemberDefinition(BaseNode node, string type)
			: this(node, type, 0)
		{

		}

		public MemberDefinition(BaseNode node, string type, int arrayCount)
			: this(type, arrayCount, node.Name, node.Offset.ToInt32(), node.Comment)
		{
			Contract.Requires(node != null);
			Contract.Requires(type != null);
		}

		public MemberDefinition(string type, int arrayCount, string name, int offset, string comment)
		{
			Contract.Requires(type != null);
			Contract.Requires(name != null);
			Contract.Requires(comment != null);

			ArrayCount = arrayCount;

			Offset = offset;

			Type = type;
			Name = name;
			Comment = comment;
		}
	}
}
