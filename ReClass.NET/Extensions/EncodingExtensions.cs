using System;
using System.Text;

namespace ReClassNET.Extensions
{
	public static class EncodingExtension
	{
		/// <summary>Gets the (perhaps wrong) byte count per character. Special characters may need more bytes.</summary>
		/// <param name="encoding">The encoding.</param>
		/// <returns>The byte count per character.</returns>
		public static int GuessByteCountPerChar(this Encoding encoding)
		{
			if (encoding.IsSameCodePage(Encoding.UTF8) || encoding.CodePage == 1252 /* Windows-1252 */ || encoding.IsSameCodePage(Encoding.ASCII)) return 1;
			if (encoding.IsSameCodePage(Encoding.Unicode) || encoding.IsSameCodePage(Encoding.BigEndianUnicode)) return 2;
			if (encoding.IsSameCodePage(Encoding.UTF32)) return 4;

			throw new NotImplementedException();
		}

		/// <summary>
		/// Checks if the code page of both encodings is equal.
		/// </summary>
		/// <param name="encoding"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static bool IsSameCodePage(this Encoding encoding, Encoding other)
		{
			return encoding.CodePage == other.CodePage;
		}
	}
}
