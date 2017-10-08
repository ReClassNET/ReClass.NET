using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using ReClassNET.Input;

namespace ReClassNET.UI
{
	[Designer(typeof(HotkeyBoxDesigner))]
	public partial class HotkeyBox : UserControl
	{
		public KeyboardInput Input { get; set; }

		public KeyboardHotkey Hotkey { get; } = new KeyboardHotkey();

		public HotkeyBox()
		{
			InitializeComponent();

			DisplayHotkey();
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, 20, specified);
		}

		private void textBox_Enter(object sender, EventArgs e)
		{
			timer.Enabled = true;
		}

		private void textBox_Leave(object sender, EventArgs e)
		{
			timer.Enabled = false;
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (Input == null)
			{
				return;
			}

			var keys = Input.GetPressedKeys();
			if (keys.Length != 0)
			{
				foreach (var key in keys.Select(k => k & Keys.KeyCode).Where(k => k != Keys.None))
				{
					Hotkey.AddKey(key);
				}
				DisplayHotkey();
			}
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			Clear();
		}

		private void DisplayHotkey()
		{
			textBox.Text = Hotkey.ToString();
		}

		public void Clear()
		{
			Hotkey.Clear();

			DisplayHotkey();
		}
	}

	internal class HotkeyBoxDesigner : ControlDesigner
	{
		private HotkeyBoxDesigner()
		{
			AutoResizeHandles = true;
		}

		public override SelectionRules SelectionRules => SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
	}
}
