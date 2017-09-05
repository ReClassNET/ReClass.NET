using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher.Algorithm
{
	public partial class SimplePatternMatcher
	{
		[ContractClass(typeof(MatchStrategyContract))]
		private interface IMatchStrategy
		{
			int PatternLength { get; }

			IEnumerable<int> SearchMatches(IList<byte> data, int index, int count);
		}

		[ContractClassFor(typeof(IMatchStrategy))]
		internal abstract class MatchStrategyContract : IMatchStrategy
		{
			public int PatternLength => throw new NotImplementedException();

			public IEnumerable<int> SearchMatches(IList<byte> data, int index, int count)
			{
				Contract.Requires(data != null);
				Contract.Requires(index >= 0);
				Contract.Requires(count >= 0);
				Contract.Requires(data.Count - index >= count);
				Contract.Ensures(Contract.Result<IEnumerable<int>>() != null);

				throw new NotImplementedException();
			}
		}
	}
}
