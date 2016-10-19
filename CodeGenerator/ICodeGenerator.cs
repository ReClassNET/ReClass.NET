using System.Collections.Generic;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	interface ICodeGenerator
	{

		string GetCodeFromClasses(IList<ClassNode> classes);
	}
}
