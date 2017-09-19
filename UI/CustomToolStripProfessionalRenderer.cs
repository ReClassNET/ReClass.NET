using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	internal class CustomToolStripProfessionalRenderer : ToolStripProfessionalRenderer
	{
		private readonly bool renderGrip;
		private readonly bool renderBorder;

		public CustomToolStripProfessionalRenderer(bool renderGrip, bool renderBorder)
			: base(new CustomProfessionalColorTable())
		{
			this.renderGrip = renderGrip;
			this.renderBorder = renderBorder;
		}

		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			if (renderGrip)
			{
				base.OnRenderGrip(e);
			}
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (renderBorder)
			{
				base.OnRenderToolStripBorder(e);
			}
		}

		protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
		{
			//base.OnRenderToolStripPanelBackground(e);
		}
	}

	internal class CustomProfessionalColorTable : ProfessionalColorTable
	{
		public override Color MenuStripGradientBegin => SystemColors.Control;

		public override Color MenuStripGradientEnd => SystemColors.Control;

		public override Color ToolStripGradientBegin => SystemColors.Control;

		public override Color ToolStripGradientMiddle => SystemColors.Control;

		public override Color ToolStripGradientEnd => SystemColors.Control;
	}
}
