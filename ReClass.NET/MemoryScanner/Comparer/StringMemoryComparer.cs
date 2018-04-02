using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class StringMemoryComparer : IScanComparer
	{
		public ScanCompareType CompareType => ScanCompareType.Equal;
		public bool CaseSensitive { get; }
		public Encoding Encoding { get; }
		public string Value { get; }
		public char[] Chars { get; }
		public int ValueSize { get; }

		public StringMemoryComparer(string value, Encoding encoding, bool caseSensitive)
		{
			CaseSensitive = caseSensitive;
			Encoding = encoding;
			Value = value;
			Chars = value.Select(c => caseSensitive ? c : char.ToUpperInvariant(c)).ToArray();
			ValueSize = encoding.GetByteCount(value);
		}

		public unsafe bool Compare(byte* data, out ScanResult result)
		{
			throw new NotImplementedException();
		}

		public unsafe bool Compare(byte* data, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is StringScanResult);
#endif

			return Compare(data, out result);
		}
	}
}
