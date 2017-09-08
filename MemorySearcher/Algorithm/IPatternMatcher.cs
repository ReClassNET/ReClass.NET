using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher.Algorithm
{
	[ContractClass(typeof(PatternMatcherContract))]
	public interface IPatternMatcher
	{
		IEnumerable<int> SearchMatches(byte[] data);

		IEnumerable<int> SearchMatches(byte[] data, int index, int count);
	}

	[ContractClassFor(typeof(IPatternMatcher))]
	internal abstract class PatternMatcherContract : IPatternMatcher
	{
		public IEnumerable<int> SearchMatches(byte[] data)
		{
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IEnumerable<int>>() != null);

			throw new NotImplementedException();
		}

		public IEnumerable<int> SearchMatches(byte[] data, int index, int count)
		{
			Contract.Requires(data != null);
			Contract.Requires(index >= 0);
			Contract.Requires(count >= 0);
			Contract.Requires(data.Length - index >= count);
			Contract.Ensures(Contract.Result<IEnumerable<int>>() != null);

			throw new NotImplementedException();
		}
	}
}
