using System.Collections.Generic;

namespace ReClassNET.MemorySearcher
{
	public interface IPatternMatcher
	{
		IEnumerable<int> SearchMatches(IList<byte> data);

		IEnumerable<int> SearchMatches(IList<byte> data, int index, int count);
	}
}
