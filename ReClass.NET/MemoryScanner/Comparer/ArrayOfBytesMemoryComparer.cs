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

		public unsafe bool Compare(byte* data, out ScanResult result)
		{
			result = null;

			if (byteArray != null)
			{
				for (var i = 0; i < byteArray.Length; ++i)
				{
					if (*(data + i) != byteArray[i])
					{
						return false;
					}
				}
			}
			// TODO: Use System.Span after it is available
			//else if (!bytePattern.Equals(new Span<byte>(data, ValueSize)))
			else if (!bytePattern.Equals(data))
			{
				return false;
			}

			var temp = new byte[ValueSize];
			fixed (byte* cpy = &temp[0])
			{
				for (var i = 0; i < ValueSize; ++i)
				{
					*(cpy + i) = *(data + i);
				}
			}
			result = new ArrayOfBytesScanResult(temp);

			return true;
		}

		public unsafe bool Compare(byte* data, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is ArrayOfBytesScanResult);
#endif

			return Compare(data, out result);
		}
	}
}
