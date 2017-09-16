using System;
using System.Diagnostics;
using System.Text;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class StringMemoryComparer : IMemoryComparer
	{
		public SearchCompareType CompareType => SearchCompareType.Equal;
		public bool CaseSensitive { get; }
		public Encoding Encoding { get; }
		public string Value { get; }
		public int ValueSize => Value.Length * Encoding.GetMaxByteCount(1);

		public StringMemoryComparer(string value, Encoding encoding, bool caseSensitive)
		{
			Value = value;
			Encoding = encoding;
			CaseSensitive = caseSensitive;
		}

		public bool Compare(byte[] data, int index, out SearchResult result)
		{
			result = null;

			var value = Encoding.GetString(data, index, Value.Length);

			if (!Value.Equals(value, CaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}

			result = new StringSearchResult(value, Encoding);

			return true;
		}

		public bool Compare(byte[] data, int index, SearchResult previous, out SearchResult result)
		{
#if DEBUG
			Debug.Assert(previous is StringSearchResult);
#endif

			return Compare(data, index, out result);
		}
	}
}
