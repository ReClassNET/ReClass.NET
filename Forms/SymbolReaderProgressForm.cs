using System.Windows.Forms;

namespace ReClassNET.Forms
{
	public partial class SymbolReaderProgressForm : Form
	{
		public int ProgressValue
		{
			get { return progressBar.Value; }
			set { progressBar.Value = value; }
		}

		public int ProgressMaximum
		{
			get { return progressBar.Maximum; }
			set { progressBar.Maximum = value; }
		}

		public string ProgressText
		{
			get { return label.Text; }
			set { label.Text = value; }
		}

		public SymbolReaderProgressForm()
		{
			InitializeComponent();
		}
	}
}
