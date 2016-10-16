using ReClassNET.Nodes;
using System.Collections.Generic;

namespace ReClassNET.CodeGenerator
{
	interface ICodeGenerator
	{

		string GetCodeFromClasses(IList<ClassNode> classes);
	}
}
