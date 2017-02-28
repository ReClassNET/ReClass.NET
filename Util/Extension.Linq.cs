using System.Collections.Generic;

namespace ReClassNET.Util
{
	public static class LinqExtension
	{
		public static string Join(this IEnumerable<string> source)
		{
			return Join(source, string.Empty);
		}

		public static string Join(this IEnumerable<string> source, string separator)
		{
			return string.Join(separator, source);
		}
	}
}
