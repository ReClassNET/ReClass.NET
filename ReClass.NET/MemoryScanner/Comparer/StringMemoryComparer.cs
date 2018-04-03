using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class StringMemoryComparer : IScanComparer
	{
		public ScanCompareType CompareType => ScanCompareType.Equal;
		public bool IsCaseSensitive { get; }
		public Encoding Encoding { get; }
		public string Value { get; }
		public char[] Chars { get; }
		public int ValueSize { get; }

		private unsafe delegate char GetCharacter(byte* data, int index);

		private readonly GetCharacter getCharacter;

		public StringMemoryComparer(string value, Encoding encoding, bool isCaseSensitive)
		{
			IsCaseSensitive = isCaseSensitive;
			Encoding = encoding;
			Value = value;
			Chars = value.Select(c => isCaseSensitive ? c : char.ToUpperInvariant(c)).ToArray();
			ValueSize = Encoding.GetByteCount(value);

			unsafe
			{
				getCharacter = Encoding == Encoding.UTF8
					? IsCaseSensitive
						? (GetCharacter)GetCharacterCaseSensitiveUtf8
						: GetCharacterCaseInsensitiveUtf8
					: Encoding == Encoding.Unicode
						? IsCaseSensitive
							? (GetCharacter)GetCharacterCaseSensitiveUtf16
							: GetCharacterCaseInsensitiveUtf16
						: IsCaseSensitive
							? (GetCharacter)GetCharacterCaseSensitiveUtf32
							: GetCharacterCaseInsensitiveUtf32;
			}
		}

		private static unsafe char GetCharacterCaseSensitiveUtf8(byte* data, int index)
		{
			return (char)*(data + index);
		}

		private static unsafe char GetCharacterCaseInsensitiveUtf8(byte* data, int index)
		{
			return char.ToUpperInvariant(GetCharacterCaseSensitiveUtf8(data, index));
		}

		private static unsafe char GetCharacterCaseSensitiveUtf16(byte* data, int index)
		{
			return *((char*)data + index);
		}

		private static unsafe char GetCharacterCaseInsensitiveUtf16(byte* data, int index)
		{
			return char.ToUpperInvariant(GetCharacterCaseSensitiveUtf16(data, index));
		}

		private static unsafe char GetCharacterCaseSensitiveUtf32(byte* data, int index)
		{
			var dst = stackalloc char[1];
			Encoding.UTF32.GetChars(data + index * sizeof(int), 4, dst, 1);
			return *dst;
		}

		private static unsafe char GetCharacterCaseInsensitiveUtf32(byte* data, int index)
		{
			return char.ToUpperInvariant(GetCharacterCaseSensitiveUtf32(data, index));
		}

		public unsafe bool Compare(byte* data, out ScanResult result)
		{
			result = null;

			for (var i = 0; i < Chars.Length; ++i)
			{
				if (Chars[i] != getCharacter(data, i))
				{
					return false;
				}
			}

			result = new StringScanResult(Value, Encoding);

			return true;
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
