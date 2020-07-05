using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using ReClassNET.MemoryScanner.Comparer;

namespace ReClassNET.MemoryScanner
{
	internal class SimpleScannerWorker : IScannerWorker
	{
		private readonly ScanSettings settings;
		private readonly ISimpleScanComparer comparer;

		public SimpleScannerWorker(ScanSettings settings, ISimpleScanComparer comparer)
		{
			Contract.Requires(settings != null);
			Contract.Requires(comparer != null);

			this.settings = settings;
			this.comparer = comparer;
		}

		public IList<ScanResult> Search(byte[] data, int count, CancellationToken ct)
		{
			Contract.Requires(data != null);

			var results = new List<ScanResult>();

			var endIndex = count - comparer.ValueSize;

			for (var i = 0; i < endIndex; i += settings.FastScanAlignment)
			{
				if (ct.IsCancellationRequested)
				{
					break;
				}

				if (comparer.Compare(data, i, out var result))
				{
					result.Address = (IntPtr)i;

					results.Add(result);
				}
			}

			return results;
		}

		public IList<ScanResult> Search(byte[] data, int count, IEnumerable<ScanResult> previousResults, CancellationToken ct)
		{
			Contract.Requires(data != null);
			Contract.Requires(previousResults != null);

			var results = new List<ScanResult>();

			var endIndex = count - comparer.ValueSize;

			foreach (var previousResult in previousResults)
			{
				if (ct.IsCancellationRequested)
				{
					break;
				}

				var offset = previousResult.Address.ToInt32();
				if (offset <= endIndex)
				{
					if (comparer.Compare(data, offset, previousResult, out var result))
					{
						result.Address = previousResult.Address;

						results.Add(result);
					}
				}
			}

			return results;
		}
	}
}
