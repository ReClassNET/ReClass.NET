using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class ArrayOfBytesMemoryComparer : IMemoryComparer
	{
		public SearchCompareType CompareType => SearchCompareType.Equal;
		public BytePattern Value { get; }
		public int ValueSize => Value.Length;

		private readonly byte[] pattern;

		public ArrayOfBytesMemoryComparer(BytePattern value)
		{
			Contract.Requires(value != null);

			Value = value;

			if (!value.HasWildcards)
			{
				pattern = value.ToByteArray();
			}
		}

		public bool Compare(byte[] data, int index, out SearchResult result)
		{
			result = null;

			if (pattern != null)
			{
				for (var i = 0; i < pattern.Length; ++i)
				{
					if (data[index + i] != pattern[i])
					{
						return false;
					}
				}
			}
			else if (!Value.Equals(data, index))
			{
				return false;
			}

			var temp = new byte[ValueSize];
			Array.Copy(data, index, temp, 0, temp.Length);
			result = new ArrayOfBytesSearchResult(temp);

			return true;
		}

		public bool Compare(byte[] data, int index, SearchResult previous, out SearchResult result)
		{
#if DEBUG
			Debug.Assert(previous is ArrayOfBytesSearchResult);
#endif

			return Compare(data, index, out result);
		}
	}
}
