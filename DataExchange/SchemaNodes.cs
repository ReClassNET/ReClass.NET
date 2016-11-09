using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.DataExchange
{
	[Serializable]
	public class SchemaNode
	{
		public SchemaType Type { get; }
		public string Name { get; set; }
		public string Comment { get; set; }
		public int Count { get; set; } = -1;

		public SchemaNode(SchemaType type)
		{
			Type = type;
		}
	}

	[Serializable]
	public class SchemaReferenceNode : SchemaNode
	{
		public SchemaClassNode InnerNode { get; }

		public SchemaReferenceNode(SchemaType type, SchemaClassNode inner)
			: base(type)
		{
			Contract.Requires(inner != null);

			InnerNode = inner;
		}
	}

	[Serializable]
	public class SchemaVTableNode : SchemaNode
	{
		public List<SchemaNode> Nodes { get; } = new List<SchemaNode>();

		public SchemaVTableNode()
			: base(SchemaType.VTable)
		{

		}
	}

	[Serializable]
	public class SchemaClassNode : SchemaNode
	{
		public string AddressFormula { get; set; }
		public List<SchemaNode> Nodes { get; } = new List<SchemaNode>();

		public SchemaClassNode()
			: base(SchemaType.Class)
		{

		}
	}

	public class SchemaClassNodeEqualityComparer : IEqualityComparer<SchemaClassNode>
	{
		public bool Equals(SchemaClassNode c1, SchemaClassNode c2)
		{
			if (c2 == null && c1 == null)
			{
				return true;
			}
			if (c1 == null | c2 == null)
			{
				return false;
			}
			if (c1.Name == c2.Name)
			{
				return true;
			}

			return false;
		}

		public int GetHashCode(SchemaClassNode c)
		{
			return c.Name.GetHashCode();
		}
	}
}
