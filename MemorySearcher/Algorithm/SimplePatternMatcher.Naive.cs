using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher.Algorithm
{
	public partial class SimplePatternMatcher
	{
		private class NaiveMatchStrategy : IMatchStrategy
		{
			private readonly byte[] pattern;
			private readonly int stepSize;

			public NaiveMatchStrategy(byte[] pattern)
			{
				Contract.Requires(pattern != null);

				this.pattern = pattern;
			}

			public NaiveMatchStrategy(byte[] pattern, int stepSize)
			{
				Contract.Requires(pattern != null);

				this.pattern = pattern;
				this.stepSize = stepSize;
			}

			public IEnumerable<int> SearchMatches(IList<byte> data, int index, int count)
			{
				var patternLength = pattern.Length;
				if (count < patternLength)
				{
					yield break;
				}

				var endIndex = index + count - patternLength + 1;

				for (var i = index; i < endIndex; i += stepSize)
				{
					var found = true;
					for (var j = 0; j < patternLength; ++j)
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
