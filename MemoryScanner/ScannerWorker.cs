using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ReClassNET.MemoryScanner.Comparer;

namespace ReClassNET.MemoryScanner
{
	internal class ScannerWorker
	{
		private readonly ScanSettings settings;
		private readonly IScanComparer comparer;

		public ScannerWorker(ScanSettings settings, IScanComparer comparer)
		{
			Contract.Requires(settings != null);
			Contract.Requires(comparer != null);

			this.settings = settings;
			this.comparer = comparer;
		}

		public IEnumerable<ScanResult> Search(byte[] data, int count)
		{
			Contract.Requires(data != null);

			var endIndex = count - comparer.ValueSize;

			for (var i = 0; i < endIndex; i += settings.FastScanAlignment)
			{
				if (comparer.Compare(data, i, out var result))
				{
					result.Address = (IntPtr)i;

					yield return result;
				}
			}
		}

		public IEnumerable<ScanResult> Search(byte[] data, int count, IEnumerable<ScanResult> results)
		{
			Contract.Requires(data != null);
			Contract.Requires(results != null);

			var endIndex = count - comparer.ValueSize;

			foreach (var previous in results)
			{
				var offset = previous.Address.ToInt32();
				if (offset + comparer.ValueSize < count)
				{
					if (comparer.Compare(data, offset, previous, out var result))
					{
						result.Address = previous.Address;

						yield return result;
					}
				}
			}
		}
	}
}
