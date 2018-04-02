using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
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

		/// <summary>
		/// Uses the <see cref="IScanComparer"/> to scan the byte array for results.
		/// </summary>
		/// <param name="data">The data to scan.</param>
		/// <param name="count">The length of the <paramref name="data"/> parameter.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <returns>An enumeration of all <see cref="ScanResult"/>s.</returns>
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

		/// <summary>
		/// Uses the <see cref="IScanComparer"/> to scan the byte array for results.
		/// The comparer uses the provided previous results to compare to the current value.
		/// </summary>
		/// <param name="data">The data to scan.</param>
		/// <param name="count">The length of the <paramref name="data"/> parameter.</param>
		/// <param name="previousResults">The previous results to use.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <returns>An enumeration of all <see cref="ScanResult"/>s.</returns>
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
