using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher
{
	public partial class SimplePatternMatcher
	{
		private class RabinKarpMatchStrategy : IMatchStrategy
		{
			private const uint PrimeBase = 37;

			private readonly byte[] pattern;

			private readonly uint power;
			private readonly uint patternHash;

			public int PatternLength => pattern.Length;

			public RabinKarpMatchStrategy(byte[] pattern)
			{
				Contract.Requires(pattern != null);

				this.pattern = pattern;

				power = 1;
				for (var i = 0; i < pattern.Length; i++)
				{
					power *= PrimeBase;
				}

				patternHash = CalculateHash(pattern, 0, pattern.Length);
			}

			private static uint CalculateHash(IList<byte> data, int index, int count)
			{
				Contract.Requires(data != null);
				Contract.Requires(index >= 0);
				Contract.Requires(count >= 0);
				Contract.Requires(data.Count - index >= count);

				uint hash = 0;
				for (var i = index; i < index + count; ++i)
				{
					hash = hash * PrimeBase + data[i];
				}
				return hash;
			}

			private uint UpdateHash(uint hash, byte outByte, byte inByte)
			{
				unchecked
				{
					hash = PrimeBase * hash + inByte - power * outByte;
				}
				return hash;
			}

			public IEnumerable<int> SearchMatches(IList<byte> data, int index, int count)
			{
				if (count < PatternLength)
				{
					yield break;
				}

				var dataHash = CalculateHash(data, index, PatternLength);

				var endIndex = index + count - PatternLength;

				for (var i = index; i < endIndex; ++i)
				{
					if (dataHash == patternHash)
					{
						yield return i - index;
					}

					dataHash = UpdateHash(dataHash, data[i], data[i + PatternLength]);
				}
			}
		}
	}
}
