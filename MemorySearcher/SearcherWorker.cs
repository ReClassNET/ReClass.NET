using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReClassNET.MemorySearcher
{
	internal class SearcherWorker
	{
		private static int InstanceCounter = 0;

		private const int MaxResultCount = 200000;

		private readonly int id;

		private readonly SearchSettings settings;
		private readonly List<SearchResult> results = new List<SearchResult>(2000);

		public SearcherWorker(SearchSettings settings)
		{
			Contract.Requires(settings != null);

			this.settings = settings;

			id = Interlocked.Increment(ref InstanceCounter);
		}

		public void Search(IntPtr address, byte[] data)
		{
			var endIndex = data.Length - settings.Comparer.ValueSize;

			var comparer = settings.Comparer;

			for (var i = 0; i < endIndex; i += settings.FastScanAlignment)
			{
				if (comparer.Compare(data, i, out var result))
				{
					result.Address = address + i;

					results.Add(result);
				}
			}
		}

		public void Finish()
		{
			
		}
	}
}
