using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class RegexStringMemoryComparer : IComplexScanComparer
	{
		public ScanCompareType CompareType => ScanCompareType.Equal;

		public Regex Pattern { get; }

		public Encoding Encoding { get; }

		public RegexStringMemoryComparer(string pattern, Encoding encoding, bool caseSensitive)
		{
			var options = RegexOptions.Singleline | RegexOptions.Compiled;
			if (!caseSensitive)
			{
				options |= RegexOptions.IgnoreCase;
			}

			Pattern = new Regex(pattern, options);

			Encoding = encoding;
		}

		public IEnumerable<ScanResult> Compare(byte[] data, int size)
		{
			var buffer = Encoding.GetString(data, 0, size);
			var bufferArray = buffer.ToCharArray();

			var lastIndex = 0;
			var lastOffset = 0;

			var match = Pattern.Match(buffer);
			while (match.Success)
			{
				var byteOffset = Encoding.GetByteCount(bufferArray, lastIndex, match.Index - lastIndex) + lastOffset;

				lastIndex = match.Index;
				lastOffset = byteOffset;

				yield return new RegexStringScanResult(match.Value, Encoding)
				{
					Address = (IntPtr)byteOffset
				};

				match = match.NextMatch();
			}
		}

		public bool CompareWithPrevious(byte[] data, int size, ScanResult previous, out ScanResult result)
		{
			result = null;

			var byteOffset = previous.Address.ToInt32();
			if (byteOffset >= size)
			{
				return false;
			}

			var buffer = Encoding.GetString(data, byteOffset, size - byteOffset);

			var match = Pattern.Match(buffer);
			if (!match.Success)
			{
				return false;
			}

			result = new RegexStringScanResult(match.Value, Encoding)
			{
				Address = (IntPtr)byteOffset
			};

			return true;
		}
	}
}
