using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class ArrayOfBytesMemoryComparer : IScanComparer
	{
		public ScanCompareType CompareType => ScanCompareType.Equal;
		public int ValueSize => bytePattern?.Length ?? byteArray.Length;

		private readonly BytePattern bytePattern;
		private readonly byte[] byteArray;

		public ArrayOfBytesMemoryComparer(BytePattern pattern)
		{
			Contract.Requires(pattern != null);

			bytePattern = pattern;

			if (!bytePattern.HasWildcards)
			{
				byteArray = bytePattern.ToByteArray();
			}
		}

		public ArrayOfBytesMemoryComparer(byte[] pattern)
		{
			Contract.Requires(pattern != null);

			byteArray = pattern;
		}

		public bool Compare(byte[] data, int index, out ScanResult result)
		{
			result = null;

			if (byteArray != null)
			{
				for (var i = 0; i < byteArray.Length; ++i)
				{
					if (data[index + i] != byteArray[i])
					{
						return false;
					}
				}
			}
			else if (!bytePattern.Equals(data, index))
			{
				return false;
			}

			var temp = new byte[ValueSize];
			Array.Copy(data, index, temp, 0, temp.Length);
			result = new ArrayOfBytesScanResult(temp);

			return true;
		}

		public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is ArrayOfBytesScanResult);
#endif

			return Compare(data, index, out result);
		}
	}
}
