using System.Diagnostics.Contracts;
using ReClassNET.MemoryScanner.Comparer;

namespace ReClassNET.MemoryScanner
{
	internal class ScannerContext
	{
		public byte[] Buffer { get; private set; }
		public ScannerWorker Worker { get; }

		public ScannerContext(ScanSettings settings, IScanComparer comparer, int bufferSize)
		{
			Contract.Requires(settings != null);
			Contract.Requires(comparer != null);
			Contract.Requires(bufferSize >= 0);
			Contract.Ensures(Buffer != null);
			Contract.Ensures(Worker != null);

			EnsureBufferSize(bufferSize);

			Worker = new ScannerWorker(settings, comparer);
		}

		public void EnsureBufferSize(int size)
		{
			Contract.Requires(size >= 0);
			Contract.Ensures(Buffer != null);

			if (Buffer == null || Buffer.Length < size)
			{
				Buffer = new byte[size];
			}
		}
	}
}
