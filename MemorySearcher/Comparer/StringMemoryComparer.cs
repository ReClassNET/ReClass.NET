using System;
using System.Diagnostics;
using System.Text;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class StringMemoryComparer : IMemoryComparer
	{
		public SearchCompareType CompareType => SearchCompareType.Equal;
		public bool CaseSensitive { get; } = true;
		public Encoding Encoding { get; }
		public string Value { get; }
		public int ValueSize => Value.Length * Encoding.GetMaxByteCount(1);

		public StringMemoryComparer(string value, Encoding encoding, bool caseSensitive)
		{
			Value = value;
			Encoding = encoding;
			CaseSensitive = caseSensitive;
		}

		public bool Compare(byte[] data, int index)
		{
			var value = Encoding.GetString(data, index, Value.Length);

			return Value.Equals(value, CaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase);
		}

		public bool Compare(byte[] data, int index, SearchResult other)
		{
#if DEBUG
			Debug.Assert(other is StringSearchResult);
#endif

			return Compare(data, index);
		}
	}
}
