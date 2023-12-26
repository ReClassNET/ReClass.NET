using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Debugger;
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

		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			// Checks if we're on some address.
			var selectionPredicate = (char c) =>
			{
				c = Char.ToLower(c);
				return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z');
			};

			if (keyData == (Keys.Control | Keys.Left))
			{
				if (SelectionStart > 0 && !string.IsNullOrEmpty(Text))
				{
					var atEnd = SelectionStart == Text.Length;
					var selectionInText = Math.Min(SelectionStart, Text.Length - 1);
					var currChar = () => Text[selectionInText - 1];
					bool currMatchesPredicate = (atEnd && selectionPredicate(Text[selectionInText])) || selectionPredicate(currChar());

					if (currMatchesPredicate)
					{
						while (selectionInText > 0 && selectionPredicate(currChar()))
						{
							selectionInText -= 1;
						}
					}
					else
					{
						selectionInText -= 1;
						while (selectionInText > 0 && !selectionPredicate(currChar()))
						{
							selectionInText -= 1;
						}
					}

					selectionInText = Math.Max(selectionInText, 0);
					SelectionStart = selectionInText;
					SelectionLength = 0;

					return true;
				}
			}
			else if (keyData == (Keys.Control | Keys.Right))
			{
				var maxSelectionStart = Text.Length;
				if (!string.IsNullOrEmpty(Text) && SelectionStart != maxSelectionStart)
				{
					var selectionInText = Math.Min(SelectionStart, Text.Length - 1);
					var currChar = () => Text[selectionInText];
					bool currMatchesPredicate = selectionPredicate(currChar());

					if (currMatchesPredicate)
					{
						while (selectionInText < maxSelectionStart && selectionPredicate(currChar()))
						{
							selectionInText += 1;
						}
					}
					else
					{
						selectionInText += 1;
						while (selectionInText < maxSelectionStart && !selectionPredicate(currChar()))
						{
							selectionInText += 1;
						}
					}

					selectionInText = Math.Min(selectionInText, maxSelectionStart);
					SelectionStart = selectionInText;
					SelectionLength = 0;

					return true;
				}
			}

			return base.ProcessCmdKey(ref m, keyData);
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

		#endregion Events

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