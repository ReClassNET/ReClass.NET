using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET
{
	class CustomToolStripProfessionalRenderer : ToolStripProfessionalRenderer
	{
		private bool renderBorder;

		public CustomToolStripProfessionalRenderer(bool renderBorder)
			: base(new CustomProfessionalColorTable())
		{
			this.renderBorder = renderBorder;
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (renderBorder)
			{
				base.OnRenderToolStripBorder(e);
			}
		}
	}

	class CustomProfessionalColorTable : ProfessionalColorTable
	{
		public override Color MenuStripGradientBegin => SystemColors.Control;

		public override Color MenuStripGradientEnd => SystemColors.Control;

		public override Color ToolStripGradientBegin => SystemColors.Control;

		public override Color ToolStripGradientMiddle => SystemColors.Control;

		public override Color ToolStripGradientEnd => SystemColors.Control;
	}
}
