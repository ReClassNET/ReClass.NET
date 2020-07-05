using System.Diagnostics.Contracts;

namespace ReClassNET.MemoryScanner
{
	internal class ScannerContext
	{
		public byte[] Buffer { get; private set; }
		public IScannerWorker Worker { get; }

		public ScannerContext(IScannerWorker worker, int bufferSize)
		{
			Contract.Requires(bufferSize >= 0);
			Contract.Ensures(Buffer != null);
			Contract.Ensures(Worker != null);

			EnsureBufferSize(bufferSize);

			Worker = worker;
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
