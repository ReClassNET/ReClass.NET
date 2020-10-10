using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ReClassNET.UI;

namespace ReClassNET.Controls
{
	public class HotSpotTextBox : TextBox
	{
		private HotSpot currentHotSpot;

		private FontEx font;
		private int minimumWidth;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new FontEx Font
		{
			get => font;
			set
			{
				if (font != value)
				{
					font = value;

					base.Font = font.Font;
				}
			}
		}

		public event HotSpotTextBoxCommitEventHandler Committed;

		public HotSpotTextBox()
		{
			BorderStyle = BorderStyle.None;
		}

		#region Events

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (Visible)
			{
				BackColor = Program.Settings.BackgroundColor;

				if (currentHotSpot != null)
				{
					Focus();
					Select(0, TextLength);
				}
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				OnCommit();

				e.Handled = true;
				e.SuppressKeyPress = true;
			}

			base.OnKeyDown(e);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			var w = (TextLength + 1) * font.Width;
			if (w > minimumWidth)
			{
				Width = w;
			}
		}

		private void OnCommit()
		{
			Visible = false;

			currentHotSpot.Text = Text.Trim();

			Committed?.Invoke(this, new HotSpotTextBoxCommitEventArgs(currentHotSpot));
		}

		#endregion

		public void ShowOnHotSpot(HotSpot hotSpot)
		{
			currentHotSpot = hotSpot;

			if (hotSpot == null)
			{
				Visible = false;

				return;
			}

			AlignToRect(hotSpot.Rect);

			Text = hotSpot.Text.Trim();
			ReadOnly = hotSpot.Id == HotSpot.ReadOnlyId;

			Visible = true;
		}

		private void AlignToRect(Rectangle rect)
		{
			SetBounds(rect.Left + 2, rect.Top, rect.Width, rect.Height);

			minimumWidth = rect.Width;
		}
	}

	public delegate void HotSpotTextBoxCommitEventHandler(object sender, HotSpotTextBoxCommitEventArgs e);

	public class HotSpotTextBoxCommitEventArgs : EventArgs
	{
		public HotSpot HotSpot { get; set; }

		public HotSpotTextBoxCommitEventArgs(HotSpot hotSpot)
		{
			HotSpot = hotSpot;
		}
	}
}
