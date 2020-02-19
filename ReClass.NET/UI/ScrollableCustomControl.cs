using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	public class ScrollableCustomControl : UserControl
	{
		private const int WM_HSCROLL = 0x114;
		private const int WM_VSCROLL = 0x115;
		private const int SBS_HORZ = 0x0000;
		private const int SBS_VERT = 0x0001;
		private const int SIF_ALL = 0x0001 | 0x0002 | 0x0004 | 0x0010;

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern bool GetScrollInfo(HandleRef hWnd, int fnBar, SCROLLINFO si);

		[StructLayout(LayoutKind.Sequential)]
		private class SCROLLINFO
		{
			public int cbSize = Marshal.SizeOf(typeof(SCROLLINFO));
			public int fMask = SIF_ALL;
			public int nMin;
			public int nMax;
			public int nPage;
			public int nPos;
			public int nTrackPos;
		}

		private readonly SCROLLINFO scrollinfo = new SCROLLINFO();

		public ScrollableCustomControl()
		{
			VScroll = true;
			HScroll = true;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			Contract.Assume(VerticalScroll != null);
			Contract.Assume(HorizontalScroll != null);

			const int WHEEL_DELTA = 120;

			var scrollProperties = VerticalScroll.Enabled ? VerticalScroll : (ScrollProperties)HorizontalScroll;

			var wheelDelta = e.Delta;
			while (Math.Abs(wheelDelta) >= WHEEL_DELTA)
			{
				if (wheelDelta > 0)
				{
					wheelDelta -= WHEEL_DELTA;
					DoScroll(ScrollEventType.SmallDecrement, scrollProperties);
				}
				else
				{
					wheelDelta += WHEEL_DELTA;
					DoScroll(ScrollEventType.SmallIncrement, scrollProperties);
				}
			}

			base.OnMouseWheel(e);
		}

		private const int SB_LINEUP = 0;
		private const int SB_LINEDOWN = 1;
		private const int SB_PAGEUP = 2;
		private const int SB_PAGEDOWN = 3;
		private const int SB_THUMBPOSITION = 4;
		private const int SB_THUMBTRACK = 5;
		private const int SB_TOP = 6;
		private const int SB_BOTTOM = 7;
		private const int SB_ENDSCROLL = 8;

		private static ScrollEventType WParamToScrollEventType(IntPtr wParam)
		{
			switch (LoWord((int)wParam))
			{
				case SB_LINEUP:
					return ScrollEventType.SmallDecrement;
				case SB_LINEDOWN:
					return ScrollEventType.SmallIncrement;
				case SB_PAGEUP:
					return ScrollEventType.LargeDecrement;
				case SB_PAGEDOWN:
					return ScrollEventType.LargeIncrement;
				case SB_THUMBTRACK:
					return ScrollEventType.ThumbTrack;
				case SB_TOP:
					return ScrollEventType.First;
				case SB_BOTTOM:
					return ScrollEventType.Last;
				case SB_THUMBPOSITION:
					return ScrollEventType.ThumbPosition;
				case SB_ENDSCROLL:
					return ScrollEventType.EndScroll;
				default:
					return ScrollEventType.EndScroll;
			}
		}

		private void SetValue(ScrollEventType type, ScrollProperties scrollProperties, int newValue)
		{
			Contract.Requires(scrollProperties != null);

			if (!scrollProperties.Enabled)
			{
				return;
			}

			if (newValue < scrollProperties.Minimum)
			{
				newValue = scrollProperties.Minimum;
			}
			if (newValue > scrollProperties.Maximum - scrollProperties.LargeChange)
			{
				newValue = scrollProperties.Maximum - scrollProperties.LargeChange + 1;
			}
			if (scrollProperties.Value != newValue)
			{
				var oldValue = scrollProperties.Value;

				scrollProperties.Value = newValue;

				if (type != ScrollEventType.EndScroll)
				{
					OnScroll(new ScrollEventArgs(
						type,
						oldValue,
						newValue,
						scrollProperties is VScrollProperties ? ScrollOrientation.VerticalScroll : ScrollOrientation.HorizontalScroll
					));
					Invalidate();
				}
			}
		}

		private void DoScroll(ScrollEventType type, ScrollProperties scrollProperties)
		{
			Contract.Requires(scrollProperties != null);

			var oldValue = scrollProperties.Value;
			var newValue = oldValue;

			switch (type)
			{
				case ScrollEventType.SmallDecrement:
					newValue = oldValue - (ModifierKeys == Keys.Control ? 1 : scrollProperties.SmallChange);
					break;
				case ScrollEventType.SmallIncrement:
					newValue = oldValue + (ModifierKeys == Keys.Control ? 1 : scrollProperties.SmallChange);
					break;
				case ScrollEventType.LargeDecrement:
					newValue = oldValue - scrollProperties.LargeChange;
					break;
				case ScrollEventType.LargeIncrement:
					newValue = oldValue + scrollProperties.LargeChange;
					break;
				case ScrollEventType.First:
					newValue = scrollProperties.Minimum;
					break;
				case ScrollEventType.Last:
					newValue = scrollProperties.Maximum;
					break;
			}

			SetValue(type, scrollProperties, newValue);
		}

		public void DoScroll(ScrollOrientation orientation, int amount)
		{
			if (orientation == ScrollOrientation.VerticalScroll && VerticalScroll.Enabled == false)
			{
				return;
			}
			if (orientation == ScrollOrientation.HorizontalScroll && HorizontalScroll.Enabled == false)
			{
				return;
			}

			var scrollProperties = orientation == ScrollOrientation.VerticalScroll ? VerticalScroll : (ScrollProperties)HorizontalScroll;

			SetValue(ScrollEventType.ThumbPosition, scrollProperties, scrollProperties.Value + amount);
		}

		private void ProcessMessage(ref Message msg)
		{
			ScrollProperties scrollProperties;
			int bar;
			if (msg.Msg == WM_VSCROLL)
			{
				scrollProperties = VerticalScroll;
				bar = SBS_VERT;
			}
			else
			{
				scrollProperties = HorizontalScroll;
				bar = SBS_HORZ;
			}

			var type = WParamToScrollEventType(msg.WParam);
			switch (type)
			{
				case ScrollEventType.SmallDecrement:
				case ScrollEventType.SmallIncrement:
				case ScrollEventType.LargeDecrement:
				case ScrollEventType.LargeIncrement:
				case ScrollEventType.First:
				case ScrollEventType.Last:
					DoScroll(type, scrollProperties);
					break;
				case ScrollEventType.ThumbTrack:
				case ScrollEventType.ThumbPosition:
					
					if (GetScrollInfo(new HandleRef(this, Handle), bar, scrollinfo))
					{
						SetValue(type, scrollProperties, scrollinfo.nTrackPos);
					}
					break;
			}
		}

		protected override void WndProc(ref Message msg)
		{
			if (msg.HWnd == Handle)
			{
				switch (msg.Msg)
				{
					case WM_VSCROLL:
					case WM_HSCROLL:
						if (msg.LParam != IntPtr.Zero)
						{
							break;
						}

						ProcessMessage(ref msg);

						return;
				}
			}

			base.WndProc(ref msg);
		}

		static int LoWord(int number) => number & 0xffff;
	}
}
