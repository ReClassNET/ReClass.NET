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
			if (encoding.Equals(Encoding.UTF8) || encoding.Equals(Encoding.ASCII)) return 1;
			if (encoding.Equals(Encoding.Unicode) || encoding.Equals(Encoding.BigEndianUnicode)) return 2;
			if (encoding.Equals(Encoding.UTF32)) return 4;

			throw new NotImplementedException();
		}
	}
}
