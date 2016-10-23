using System.Collections.Generic;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	public interface ICodeGenerator
	{
		string GetCodeFromClasses(IEnumerable<ClassNode> classes);
	}
}
