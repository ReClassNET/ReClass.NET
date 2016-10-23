using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ReClassNET.CodeGenerator;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class CodeForm : Form
	{
		public CodeForm(ICodeGenerator generator, IList<ClassNode> classes)
		{
			Contract.Requires(generator != null);
			Contract.Requires(classes != null);

			InitializeComponent();

			var s = generator.GetCodeFromClasses(classes);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			GlobalWindowManager.RemoveWindow(this);
		}
	}
}
