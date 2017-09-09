using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class ArrayOfBytesMemoryComparer : IMemoryComparer
	{
		public SearchCompareType CompareType { get; set; } = SearchCompareType.Equal;
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

		public bool Compare(byte[] data, int index)
		{
			if (pattern != null)
			{
				for (var i = 0; i < pattern.Length; ++i)
				{
					if (data[index + i] != pattern[i])
					{
						return false;
					}
				}

				return true;
			}
			else
			{
				return Value.Equals(data, index);
			}
		}

		public bool Compare(byte[] data, int index, SearchResult other)
		{
#if DEBUG
			Debug.Assert(other is ArrayOfBytesSearchResult);
#endif

			return Compare(data, index);
		}
	}
}
