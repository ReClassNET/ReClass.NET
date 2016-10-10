using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET
{
	class Settings
	{
		[Category("Colors")]
		public Color Background { get; set; } = Color.FromArgb(255, 255, 255);
		public Color Selected { get; set; } = Color.FromArgb(240, 240, 240);
		public Color Hidden { get; set; } = Color.FromArgb(240, 240, 240);
		public Color Offset { get; set; } = Color.FromArgb(255, 0, 0);
		public Color Address { get; set; } = Color.FromArgb(0, 200, 0);
		public Color Hex { get; set; } = Color.FromArgb(0, 0, 0);
		public Color Type { get; set; } = Color.FromArgb(0, 0, 255);
		public Color Name { get; set; } = Color.FromArgb(32, 32, 128);
		public Color Value { get; set; } = Color.FromArgb(255, 128, 0);
		public Color Index { get; set; } = Color.FromArgb(32, 200, 200);
		public Color Comment { get; set; } = Color.FromArgb(0, 200, 0);
		public Color Text { get; set; } = Color.FromArgb(0, 0, 255);
		public Color VTable { get; set; } = Color.FromArgb(0, 255, 0);
		public Color Function { get; set; } = Color.FromArgb(255, 0, 255);
		public Color Custom { get; set; } = Color.FromArgb(64, 128, 64);

		[Category("Display")]
		public bool ShowAddress { get; set; } = true;
		public bool ShowOffset { get; set; } = true;
		public bool ShowText { get; set; } = true;

		[Category("Comment")]
		public bool ShowFloat { get; set; } = true;
		public bool ShowInteger { get; set; } = true;
		public bool ShowPointer { get; set; } = true;
		public bool ShowRTTI { get; set; } = false;
		public bool ShowSymbols { get; set; } = false;
		public bool ShowStrings { get; set; } = true;
	}
}
