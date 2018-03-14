using System.IO;

namespace ReClassNET.Extensions
{
	public static class StringReaderExtension
	{
		public static int ReadSkipWhitespaces(this StringReader sr)
		{
			while (true)
			{
				var i = sr.Read();
				if (i == -1)
				{
					return i;
				}

				if (!char.IsWhiteSpace((char)i))
				{
					return i;
				}
			}
		}
	}
}
