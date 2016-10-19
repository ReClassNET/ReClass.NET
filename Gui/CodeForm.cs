using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.CodeGenerator;
using ReClassNET.Nodes;

namespace ReClassNET.Gui
{
	partial class CodeForm : Form
	{
		public CodeForm(ICodeGenerator generator, IList<ClassNode> classes)
		{
			Contract.Requires(generator != null);
			Contract.Requires(classes != null);

			InitializeComponent();

			var s = generator.GetCodeFromClasses(classes);
		}
	}
}
