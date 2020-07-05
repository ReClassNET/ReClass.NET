namespace ReClassNET.MemoryScanner.Comparer
{
	public interface ISimpleScanComparer : IScanComparer
	{
		int ValueSize { get; }

		/// <summary>
		/// Compares the data at the provided index to the current <see cref="IScanComparer.CompareType"/>.
		/// </summary>
		/// <param name="data">The byte array to be compared.</param>
		/// <param name="index">The index into the byte array.</param>
		/// <param name="result">[out] The scan result if the <see cref="IScanComparer.CompareType"/> matched.</param>
		/// <returns>True if matched.</returns>
		bool Compare(byte[] data, int index, out ScanResult result);

		/// <summary>
		/// Compares the data at the provided index to the current <see cref="IScanComparer.CompareType"/>.
		/// The previous results may be used.
		/// </summary>
		/// <param name="data">The byte array to be compared.</param>
		/// <param name="index">The index into the byte array.</param>
		/// <param name="previous">Scan result to be compared.</param>
		/// <param name="result">[out] The scan result if the <see cref="IScanComparer.CompareType"/> matched.</param>
		/// <returns>True if matched.</returns>
		bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result);
	}
}