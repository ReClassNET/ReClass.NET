using System.Collections.Generic;
using System.Threading;
using ReClassNET.MemoryScanner.Comparer;

namespace ReClassNET.MemoryScanner
{
	internal interface IScannerWorker
	{
		/// <summary>
		/// Uses the <see cref="IScanComparer"/> to scan the byte array for results.
		/// </summary>
		/// <param name="data">The data to scan.</param>
		/// <param name="count">The length of the <paramref name="data"/> parameter.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <returns>An enumeration of all <see cref="ScanResult"/>s.</returns>
		IList<ScanResult> Search(byte[] data, int count, CancellationToken ct);

		/// <summary>
		/// Uses the <see cref="IScanComparer"/> to scan the byte array for results.
		/// The comparer uses the provided previous results to compare to the current value.
		/// </summary>
		/// <param name="data">The data to scan.</param>
		/// <param name="count">The length of the <paramref name="data"/> parameter.</param>
		/// <param name="previousResults">The previous results to use.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <returns>An enumeration of all <see cref="ScanResult"/>s.</returns>
		IList<ScanResult> Search(byte[] data, int count, IEnumerable<ScanResult> previousResults, CancellationToken ct);
	}
}
