using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

namespace ReClassNET.MemorySearcher.Algorithm
{
	public partial class WildcardPatternMatcher : IPatternMatcher
	{
		private readonly List<PatternByte> pattern;

		public WildcardPatternMatcher(string value)
		{
			Contract.Requires(value != null);

			pattern = new List<PatternByte>();

			using (var sr = new StringReader(value))
			{
				var pb = new PatternByte();
				while (pb.TryRead(sr))
				{
					pattern.Add(pb);
				}

				// Check if we are not at the end of the stream
				if (sr.Peek() != -1)
				{
					throw new ArgumentException();
				}
			}
		}

		public IEnumerable<int> SearchMatches(byte[] data)
		{
			return SearchMatches(data, 0, data.Length);
		}

		public IEnumerable<int> SearchMatches(byte[] data, int index, int count)
		{
			if (count < pattern.Count)
			{
				yield break;
			}

			var endIndex = index + count - pattern.Count + 1;

			for (var i = index; i < endIndex; ++i)
			{
				var found = true;
				for (var j = 0; j < pattern.Count; ++j)
				{
					if (!pattern[j].Equals(data[i + j]))
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
