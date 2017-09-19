using System;
using System.Text;

namespace ReClassNET.Util
{
	public static class EncodingExtension
	{
		/// <summary>Gets the (wrong) byte count per character. Special chars may need more bytes per character.</summary>
		/// <param name="encoding">The encoding.</param>
		/// <returns>The byte count per character.</returns>
		public static int GetSimpleByteCountPerChar(this Encoding encoding)
		{
			if (encoding == Encoding.UTF8 || encoding == Encoding.ASCII) return 1;
			if (encoding == Encoding.Unicode) return 2;
			if (encoding == Encoding.UTF32) return 4;

			throw new NotImplementedException();
		}
	}
}
