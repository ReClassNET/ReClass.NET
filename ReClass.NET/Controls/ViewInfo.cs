using System;
using System.Collections.Generic;
using System.Drawing;
using ReClassNET.Memory;

namespace ReClassNET.UI
{
	public class ViewInfo
	{
		public Settings Settings { get; set; }

		public Graphics Context { get; set; }
		public FontEx Font { get; set; }

		public RemoteProcess Process { get; set; }
		public MemoryBuffer Memory { get; set; }

		public DateTime CurrentTime { get; set; }

		public Rectangle ClientArea { get; set; }
		public List<HotSpot> HotSpots { get; set; }
		public IntPtr Address { get; set; }
		public int Level { get; set; }
		public bool MultipleNodesSelected { get; set; }

		public ViewInfo Clone()
		{
			return new ViewInfo
			{
				Settings = Settings,
				Context = Context,
				Font = Font,
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
