using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemorySearcher
{
	internal class SearchResultStore
	{
		private const int MaximumResultsCount = 10000000;

		private readonly List<SearchResultBlock> store = new List<SearchResultBlock>();

		private readonly string storePath;

		public int TotalResultCount { get; private set; }

		public SearchResultStore()
		{
			this.storePath = storePath;
		}

		public IReadOnlyList<SearchResultBlock> GetResultBlocks()
		{
			return store;
		}

		public void AddBlock(SearchResultBlock block)
		{
			Contract.Requires(block != null);

			lock (store)
			{
				TotalResultCount += block.Results.Count;

				store.Add(block);

				if (TotalResultCount > MaximumResultsCount)
				{
					TransferResults();
				}
			}
		}

		private void TransferResults()
		{
			
		}
	}
}
