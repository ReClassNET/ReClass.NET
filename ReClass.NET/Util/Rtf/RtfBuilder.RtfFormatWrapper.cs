using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace ReClassNET.Util.Rtf
{
	partial class RtfBuilder
	{
		private class RtfFormatWrapper : IDisposable
		{
			private readonly RtfBuilder builder;

			public RtfFormatWrapper(RtfBuilder builder)
			{
				Contract.Requires(builder != null);

				this.builder = builder;

				if (builder.isLocked)
				{
					return;
				}

				var buffer = builder.buffer;

				int oldLength = buffer.Length;

				if ((builder.fontStyle & FontStyle.Bold) == FontStyle.Bold)
				{
					buffer.Append(@"\b");
				}
				if ((builder.fontStyle & FontStyle.Italic) == FontStyle.Italic)
				{
					buffer.Append(@"\i");
				}
				if ((builder.fontStyle & FontStyle.Underline) == FontStyle.Underline)
				{
					buffer.Append(@"\ul");
				}
				if ((builder.fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
				{
					buffer.Append(@"\strike");
				}

				if (builder.fontSize != builder.DefaultFontSize)
				{
					buffer.AppendFormat(@"\fs{0}", builder.fontSize);
				}
				if (builder.fontIndex != 0)
				{
					buffer.AppendFormat(@"\f{0}", builder.fontIndex);
				}
				if (builder.foreColor != builder.DefaultForeColor)
				{
					buffer.AppendFormat(@"\cf{0}", builder.IndexOfColor(builder.foreColor));
				}
				if (builder.backColor != builder.DefaultBackColor)
				{
					buffer.AppendFormat(@"\highlight{0}", builder.IndexOfColor(builder.backColor));
				}

				if (buffer.Length > oldLength)
				{
					buffer.Append(" ");
				}
			}

			public void Dispose()
			{
				if (builder.isLocked)
				{
					return;
				}

				var buffer = builder.buffer;

				var oldLength = buffer.Length;

				if ((builder.fontStyle & FontStyle.Bold) == FontStyle.Bold)
				{
					buffer.Append(@"\b0");
				}
				if ((builder.fontStyle & FontStyle.Italic) == FontStyle.Italic)
				{
					buffer.Append(@"\i0");
				}
				if ((builder.fontStyle & FontStyle.Underline) == FontStyle.Underline)
				{
					buffer.Append(@"\ulnone");
				}
				if ((builder.fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
				{
					buffer.Append(@"\strike0");
				}

				builder.fontStyle = FontStyle.Regular;

				if (builder.fontSize != builder.DefaultFontSize)
				{
					builder.fontSize = builder.DefaultFontSize;

					buffer.AppendFormat(@"\fs{0} ", builder.DefaultFontSize);
				}
				if (builder.fontIndex != 0)
				{
					buffer.Append(@"\f0");

					builder.fontIndex = 0;
				}

				if (builder.foreColor != builder.DefaultForeColor)
				{
					builder.foreColor = builder.DefaultForeColor;

					buffer.Append(@"\cf0");
				}
				if (builder.backColor != builder.DefaultBackColor)
				{
					builder.backColor = builder.DefaultBackColor;

					buffer.Append(@"\highlight0");
				}

				if (buffer.Length > oldLength)
				{
					buffer.Append(" ");
				}
			}
		}
	}
}
