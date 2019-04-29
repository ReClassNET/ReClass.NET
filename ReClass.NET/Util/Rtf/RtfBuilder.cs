using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ReClassNET.Extensions;

namespace ReClassNET.Util.Rtf
{
	public partial class RtfBuilder
	{
		private static readonly char[] slashable = { '{', '}', '\\' };

		private readonly StringBuilder buffer;

		private readonly Color defaultForeColor = Color.Black;
		private readonly Color defaultBackColor = Color.Empty;
		private readonly float defaultFontSize;

		private readonly List<Color> usedColors = new List<Color>();
		private readonly List<string> usedFonts = new List<string>();

		private Color foreColor;
		private Color backColor;

		private int fontIndex;
		private float fontSize;
		private FontStyle fontStyle;

		public RtfBuilder()
			: this(RtfFont.Calibri, 22.0f)
		{

		}

		public RtfBuilder(RtfFont defaultFont, float defaultFontSize)
		{
			buffer = new StringBuilder();

			fontIndex = IndexOfFont(defaultFont);

			this.defaultFontSize = defaultFontSize;
			fontSize = defaultFontSize;

			usedColors.Add(defaultForeColor);
			usedColors.Add(defaultBackColor);

			fontStyle = FontStyle.Regular;
			foreColor = defaultForeColor;
			backColor = defaultBackColor;
		}

		public RtfBuilder Append(char value)
		{
			return Append(value.ToString());
		}

		public RtfBuilder Append(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				using (new RtfFormatWrapper(this))
				{
					value = EscapeString(value);
					if (value.IndexOf(Environment.NewLine, StringComparison.Ordinal) >= 0)
					{
						var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

						buffer.Append(string.Join(@"\line ", lines));
					}
					else
					{
						buffer.Append(value);
					}
				}
			}

			return this;
		}

		public RtfBuilder AppendLevel(int level)
		{
			buffer.AppendFormat(@"\level{0} ", level);

			return this;
		}

		public RtfBuilder AppendLine()
		{
			buffer.AppendLine(@"\line");

			return this;
		}

		public RtfBuilder AppendLine(string value)
		{
			Append(value);

			return AppendLine();
		}

		public RtfBuilder AppendParagraph()
		{
			buffer.AppendLine(@"\par");

			return this;
		}

		public RtfBuilder AppendPage()
		{
			buffer.AppendLine(@"\page");

			return this;
		}

		public RtfBuilder SetForeColor(Color color)
		{
			foreColor = color;

			return this;
		}

		public RtfBuilder SetBackColor(Color color)
		{
			backColor = color;

			return this;
		}

		public RtfBuilder SetFont(RtfFont font)
		{
			fontIndex = IndexOfFont(font);

			return this;
		}

		public RtfBuilder SetFontSize(float size)
		{
			fontSize = size;

			return this;
		}

		public RtfBuilder SetFontStyle(FontStyle style)
		{
			fontStyle = style;

			return this;
		}

		protected int IndexOfColor(Color color)
		{
			if (!usedColors.Contains(color))
			{
				usedColors.Add(color);
			}

			return usedColors.IndexOf(color) + 1;
		}

		private int IndexOfFont(RtfFont font)
		{
			return IndexOfRawFont(GetKnownFontString(font));
		}

		private int IndexOfRawFont(string font)
		{
			if (!string.IsNullOrEmpty(font))
			{
				var index = usedFonts.IndexOf(font);
				if (index < 0)
				{
					usedFonts.Add(font);

					return usedFonts.Count - 1;
				}
				return index;
			}
			return 0;
		}

		private static string GetKnownFontString(RtfFont font)
		{
			switch (font)
			{
				case RtfFont.Arial:
					return @"{{\f{0}\fswiss\fprq2\fcharset0 Arial;}}";
				case RtfFont.Calibri:
					return @"{{\f{0}\fnil\fcharset0 Calibri;}}";
				case RtfFont.Consolas:
					return @"{{\f{0}\fmodern\fprq1\fcharset0 Consolas;}}";
				case RtfFont.CourierNew:
					return @"{{\f{0}\fmodern\fprq1\fcharset0 Courier New;}}";
				case RtfFont.Impact:
					return @"{{\f{0}\fswiss\fprq2\fcharset0 Impact;}}";
				case RtfFont.LucidaConsole:
					return @"{{\f{0}\fmodern\fprq1\fcharset0 Lucida Console;}}";
				case RtfFont.MSSansSerif:
					return @"{{\f{0}\fswiss\fprq2\fcharset0 MS Reference Sans Serif;}}";
				case RtfFont.Symbol:
					return @"{{\f{0}\ftech\fcharset0 Symbol;}}";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public RtfBuilder Reset()
		{
			buffer.AppendLine(@"\pard");

			return this;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(@"{\rtf1\ansi\ansicpg1252\deff0\deflang3081");

			sb.Append(@"{\fonttbl");
			for (var i = 0; i < usedFonts.Count; ++i)
			{
				sb.AppendFormat(usedFonts[i], i);
			}
			sb.AppendLine("}");

			sb.Append(@"{\colortbl ;");
			foreach (var color in usedColors)
			{
				sb.Append($@"\red{color.R}\green{color.G}\blue{color.B};");
			}
			sb.AppendLine("}");

			sb.Append(@"\viewkind4\uc1\pard\plain\f0");

			sb.AppendFormat(@"\fs{0} ", defaultFontSize);
			sb.AppendLine();

			sb.Append(buffer);
			sb.Append("}");

			return sb.ToString();
		}

		private static string EscapeString(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOfAny(slashable) >= 0)
				{
					value = value.Replace("\\", "\\\\").Replace("{", @"\{").Replace("}", @"\}");
				}

				if (value.Any(c => c > 255))
				{
					var sb = new StringBuilder();

					foreach (var c in value)
					{
						if (c <= 255)
						{
							sb.Append(c);
						}
						else if (c == '\t')
						{
							sb.Append(@"\tab");
						}
						else
						{
							sb.Append(@"\u");
							sb.Append((int)c);
							sb.Append("?");
						}
					}
					value = sb.ToString();
				}
			}

			return value;
		}
	}
}