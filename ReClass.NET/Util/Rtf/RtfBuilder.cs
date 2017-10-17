using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ReClassNET.Util.Rtf
{
	public partial class RtfBuilder
	{
		private static readonly char[] slashable = new[] { '{', '}', '\\' };

		private readonly StringBuilder buffer;

		protected readonly Color DefaultForeColor = Color.Black;
		protected readonly Color DefaultBackColor = Color.Empty;
		protected readonly float DefaultFontSize;

		private readonly List<Color> usedColors = new List<Color>();
		private readonly List<string> usedFonts = new List<string>();

		protected Color foreColor;
		protected Color backColor;

		protected int fontIndex;
		protected float fontSize;
		protected FontStyle fontStyle;

		protected bool isLocked;

		public RtfBuilder()
			: this(RtfFont.Calibri, 22.0f)
		{

		}

		public RtfBuilder(RtfFont defaultFont, float defaultFontSize)
		{
			buffer = new StringBuilder();

			fontIndex = IndexOfFont(defaultFont);

			DefaultFontSize = defaultFontSize;
			fontSize = defaultFontSize;

			usedColors.Add(DefaultForeColor);
			usedColors.Add(DefaultBackColor);

			fontStyle = FontStyle.Regular;
			foreColor = DefaultForeColor;
			backColor = DefaultBackColor;
		}

		public RtfBuilder Append(char value)
		{
			return Append(value.ToString());
		}

		public RtfBuilder Append(string value)
		{
			AppendInternal(value);

			return this;
		}

		public RtfBuilder AppendLevel(int level)
		{
			AppendLevelInternal(level);

			return this;
		}

		public RtfBuilder AppendLine()
		{
			AppendLineInternal();

			return this;
		}

		public RtfBuilder AppendLine(string value)
		{
			AppendLineInternal(value);

			return this;
		}

		public RtfBuilder AppendParagraph()
		{
			buffer.AppendLine(@"\par");

			return this;
		}

		public RtfBuilder AppendPage()
		{
			AppendPageInternal();

			return this;
		}

		[DebuggerStepThrough]
		public RtfBuilder SetForeColor(Color color)
		{
			foreColor = color;

			return this;
		}

		[DebuggerStepThrough]
		public RtfBuilder SetBackColor(Color color)
		{
			backColor = color;

			return this;
		}

		[DebuggerStepThrough]
		public RtfBuilder SetFont(RtfFont font)
		{
			fontIndex = IndexOfFont(font);

			return this;
		}

		[DebuggerStepThrough]
		public RtfBuilder SetFontSize(float size)
		{
			fontSize = size;

			return this;
		}

		[DebuggerStepThrough]
		public RtfBuilder SetFontStyle(FontStyle fontStyle)
		{
			this.fontStyle = fontStyle;

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
			ResetInternal();

			return this;
		}

		protected void AppendInternal(string value)
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
		}

		protected void AppendLevelInternal(int level)
		{
			buffer.AppendFormat(@"\level{0} ", level);
		}

		protected void AppendLineInternal(string value)
		{
			Append(value);

			buffer.AppendLine(@"\line");
		}

		protected void AppendLineInternal()
		{
			buffer.AppendLine(@"\line");
		}

		protected void AppendPageInternal()
		{
			buffer.AppendLine(@"\page");
		}

		public IDisposable FormatLock()
		{
			return new RtfFormatLock(this);
		}

		protected void ResetInternal()
		{
			buffer.AppendLine(@"\pard");
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(@"{\rtf1\ansi\ansicpg1252\deff0\deflang3081");

			sb.Append(@"{\fonttbl");
			sb.Append(usedFonts.Select((f, i) => string.Format(f, i)).Join());
			sb.AppendLine("}");

			sb.Append(@"{\colortbl ;");
			sb.Append(usedColors.Select(c => $@"\red{c.R}\green{c.G}\blue{c.B};").Join());
			sb.AppendLine("}");

			sb.Append(@"\viewkind4\uc1\pard\plain\f0");

			sb.AppendFormat(@"\fs{0} ", DefaultFontSize);
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