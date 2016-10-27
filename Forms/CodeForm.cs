using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ColorCode;
using ReClassNET.CodeGenerator;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class CodeForm : IconForm
	{
		public CodeForm(ICodeGenerator generator, IEnumerable<ClassNode> classes)
		{
			Contract.Requires(generator != null);
			Contract.Requires(classes != null);

			InitializeComponent();

			BannerFactory.CreateBannerEx(bannerImage, Properties.Resources.B32x32_Page_Code, "Code Generator", "This is the code generated from the classes.");

			var code = generator.GetCodeFromClasses(classes);
			codeWebBrowser.DocumentText = new CodeColorizer().Colorize(code, generator.Language);
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
