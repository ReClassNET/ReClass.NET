using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher
{
	public partial class SimplePatternMatcher
	{
		private class NaiveMatchStrategy : IMatchStrategy
		{
			private readonly byte[] pattern;

			public int PatternLength => pattern.Length;

			public NaiveMatchStrategy(byte[] pattern)
			{
				Contract.Requires(pattern != null);

				this.pattern = pattern;
			}

			public IEnumerable<int> SearchMatches(IList<byte> data, int index, int count)
			{
				if (count < PatternLength)
				{
					yield break;
				}

				var endIndex = index + count - PatternLength + 1;

				for (var i = index; i < endIndex; ++i)
				{
					var found = true;
					for (var j = 0; j < PatternLength; ++j)
					{
						if (data[i + j] != pattern[j])
						{
							found = false;

							break;
						}
					}

					if (found)
					{
						yield return i - index;
					}
				}
			}
		}
	}
}
