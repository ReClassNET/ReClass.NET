using System.Collections.Generic;

namespace ReClassNET.MemoryScanner.Comparer
{
	public interface IComplexScanComparer : IScanComparer
	{
		/// <summary>
		/// Compares all data to the current <see cref="IScanComparer.CompareType"/>.
		/// </summary>
		/// <param name="data">The byte array to be compared.</param>
		/// <param name="size">The index into the byte array.</param>
		/// <returns>List of matches.</returns>
		IEnumerable<ScanResult> Compare(byte[] data, int size);

		/// <summary>
		/// Compares all data to the current <see cref="IScanComparer.CompareType"/>.
		/// The previous results may be used.
		/// </summary>
		/// <param name="data">The byte array to be compared.</param>
		/// <param name="size">The index into the byte array.</param>
		/// <param name="previous">Scan result to be compared.</param>
		/// <param name="result">[out] The scan result if the <see cref="IScanComparer.CompareType"/> matched.</param>
		/// <returns>True if matched.</returns>
		bool CompareWithPrevious(byte[] data, int size, ScanResult previous, out ScanResult result);
	}
}
