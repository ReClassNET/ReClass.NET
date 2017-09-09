using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ReClassNET.UI
{
	[Designer(typeof(DualValueControlDesigner))]
	public partial class DualValueControl : UserControl
	{
		public bool ShowSecondInputField
		{
			get => tableLayoutPanel.ColumnStyles[0].Width <= 99;
			set
			{
				if (value)
				{
					tableLayoutPanel.ColumnStyles[1].SizeType = SizeType.Percent;
					tableLayoutPanel.ColumnStyles[1].Width = 50;
					tableLayoutPanel.ColumnStyles[0].Width = 50;
				}
				else
				{
					tableLayoutPanel.ColumnStyles[1].SizeType = SizeType.Absolute;
					tableLayoutPanel.ColumnStyles[1].Width = 0;
					tableLayoutPanel.ColumnStyles[0].Width = 100;
					value2TextBox.Text = null;
				}
			}
		}

		public string Value1
		{
			get => value1TextBox.Text;
			set => value1TextBox.Text = value;
		}

		public string Value2
		{
			get => value2TextBox.Text;
			set => value2TextBox.Text = value;
		}

		public DualValueControl()
		{
			InitializeComponent();
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, 40, specified);
		}
	}

	internal class DualValueControlDesigner : ControlDesigner
	{
		DualValueControlDesigner()
		{
			AutoResizeHandles = true;
		}

		public override SelectionRules SelectionRules => SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
	}
}
