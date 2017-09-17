using System;
using System.Diagnostics;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class StringMemoryComparer : IMemoryComparer
	{
		public ScanCompareType CompareType => ScanCompareType.Equal;
		public bool CaseSensitive { get; }
		public Encoding Encoding { get; }
		public string Value { get; }
		public int ValueSize { get; }

		public StringMemoryComparer(string value, Encoding encoding, bool caseSensitive)
		{
			Value = value;
			Encoding = encoding;
			CaseSensitive = caseSensitive;
			ValueSize = Value.Length * Encoding.GetSimpleByteCountPerChar();
		}

		public bool Compare(byte[] data, int index, out ScanResult result)
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

		public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is StringSearchResult);
#endif

			return Compare(data, index, out result);
		}
	}
}
