using System;
using System.Collections.Generic;
using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Controls
{
	public class DrawContext
	{
		public Settings Settings { get; set; }

		public Graphics Graphics { get; set; }
		public FontEx Font { get; set; }
		public IconProvider IconProvider { get; set; }

		public RemoteProcess Process { get; set; }
		public MemoryBuffer Memory { get; set; }

		public DateTime CurrentTime { get; set; }

		public Rectangle ClientArea { get; set; }
		public List<HotSpot> HotSpots { get; set; }
		public IntPtr Address { get; set; }
		public int Level { get; set; }
		public bool MultipleNodesSelected { get; set; }

		public DrawContext Clone()
		{
			return new DrawContext
			{
				Settings = Settings,
				Graphics = Graphics,
				Font = Font,
				IconProvider = IconProvider,
				Process = Process,
				Memory = Memory,
				CurrentTime = CurrentTime,
				ClientArea = ClientArea,
				HotSpots = HotSpots,
				Address = Address,
				Level = Level,
				MultipleNodesSelected = MultipleNodesSelected
			};
		}
	}
}
