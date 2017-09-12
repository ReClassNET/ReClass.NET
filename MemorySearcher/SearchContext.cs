using System.Diagnostics.Contracts;
using ReClassNET.MemorySearcher.Comparer;

namespace ReClassNET.MemorySearcher
{
	internal class SearchContext
	{
		public byte[] Buffer { get; private set; }
		public SearcherWorker Worker { get; }

		public SearchContext(SearchSettings settings, IMemoryComparer comparer, int bufferSize)
		{
			Contract.Requires(settings != null);
			Contract.Requires(comparer != null);
			Contract.Requires(bufferSize >= 0);

			EnsureBufferSize(bufferSize);

			Worker = new SearcherWorker(settings, comparer);
		}

		public void EnsureBufferSize(int size)
		{
			Contract.Requires(size >= 0);

			if (Buffer == null || Buffer.Length < size)
			{
				Buffer = new byte[size];
			}
		}
	}
}
