using System.Collections.Generic;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	interface ICodeGenerator
	{
		string GetCodeFromClasses(IEnumerable<ClassNode> classes);
	}
}
