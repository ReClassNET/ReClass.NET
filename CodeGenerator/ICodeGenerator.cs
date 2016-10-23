using System.Collections.Generic;
using ColorCode;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	public interface ICodeGenerator
	{
		ILanguage Language { get; }

		string GetCodeFromClasses(IEnumerable<ClassNode> classes);
	}
}
