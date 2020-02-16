using System.Drawing;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET
{
	public class Settings
	{
		// Application Settings

		public string LastProcess { get; set; } = string.Empty;

		public bool StayOnTop { get; set; } = false;

		public bool RunAsAdmin { get; set; } = false;

		public bool RandomizeWindowTitle { get; set; } = false;

		// Node Drawing Settings

		public bool ShowNodeAddress { get; set; } = true;

		public bool ShowNodeOffset { get; set; } = true;

		public bool ShowNodeText { get; set; } = true;

		public bool HighlightChangedValues { get; set; } = true;

		public Encoding RawDataEncoding { get; set; } = Encoding.GetEncoding(1252); /* Windows-1252 */

		// Comment Drawing Settings

		public bool ShowCommentFloat { get; set; } = true;

		public bool ShowCommentInteger { get; set; } = true;

		public bool ShowCommentPointer { get; set; } = true;

		public bool ShowCommentRtti { get; set; } = true;

		public bool ShowCommentSymbol { get; set; } = true;

		public bool ShowCommentString { get; set; } = true;

		public bool ShowCommentPluginInfo { get; set; } = true;

		// Colors

		public Color BackgroundColor { get; set; } = Color.FromArgb(255, 255, 255);

		public Color SelectedColor { get; set; } = Color.FromArgb(240, 240, 240);

		public Color HiddenColor { get; set; } = Color.FromArgb(240, 240, 240);

		public Color OffsetColor { get; set; } = Color.FromArgb(255, 0, 0);

		public Color AddressColor { get; set; } = Color.FromArgb(0, 200, 0);

		public Color HexColor { get; set; } = Color.FromArgb(0, 0, 0);

		public Color TypeColor { get; set; } = Color.FromArgb(0, 0, 255);

		public Color NameColor { get; set; } = Color.FromArgb(32, 32, 128);

		public Color ValueColor { get; set; } = Color.FromArgb(255, 128, 0);

		public Color IndexColor { get; set; } = Color.FromArgb(32, 200, 200);

		public Color CommentColor { get; set; } = Color.FromArgb(0, 200, 0);

		public Color TextColor { get; set; } = Color.FromArgb(0, 0, 255);

		public Color VTableColor { get; set; } = Color.FromArgb(0, 255, 0);

		public Color PluginColor { get; set; } = Color.FromArgb(255, 0, 255);

		public CustomDataMap CustomData { get; } = new CustomDataMap();

		public Settings Clone() => MemberwiseClone() as Settings;
	}
}
