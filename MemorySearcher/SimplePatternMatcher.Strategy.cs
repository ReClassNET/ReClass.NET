using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher
{
	public partial class SimplePatternMatcher
	{
		[ContractClass(typeof(IMatchStrategyContract))]
		private interface IMatchStrategy
		{
			int PatternLength { get; }

			IEnumerable<int> SearchMatches(IList<byte> data, int index, int count);
		}

		[ContractClassFor(typeof(IMatchStrategy))]
		internal abstract class IMatchStrategyContract : IMatchStrategy
		{
			public int PatternLength { get { throw new NotImplementedException(); } }

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
