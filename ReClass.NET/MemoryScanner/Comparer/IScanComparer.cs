using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemoryScanner.Comparer
{
	[ContractClass(typeof(ScanComparerContract))]
	public interface IScanComparer
	{
		ScanCompareType CompareType { get; }
		int ValueSize { get; }

		/// <summary>
		/// Compares the data at the provided index to the current <see cref="CompareType"/>.
		/// </summary>
		/// <param name="data">The byte array to be compared.</param>
		/// <param name="index">The index into the byte array.</param>
		/// <param name="result">[out] The scan result if the <see cref="CompareType"/> matched.</param>
		/// <returns>True if matched.</returns>
		bool Compare(byte[] data, int index, out ScanResult result);

		/// <summary>
		/// Compares the data at the provided index to the current <see cref="CompareType"/>.
		/// The previous results may be used.
		/// </summary>
		/// <param name="data">The byte array to be compared.</param>
		/// <param name="index">The index into the byte array.</param>
		/// <param name="previous">Scan result to be compared.</param>
		/// <param name="result">[out] The scan result if the <see cref="CompareType"/> matched.</param>
		/// <returns>True if matched.</returns>
		bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result);
	}

	[ContractClassFor(typeof(IScanComparer))]
	internal abstract class ScanComparerContract : IScanComparer
	{
		public ScanCompareType CompareType => throw new NotImplementedException();

		public int ValueSize
		{
			get
			{
				Contract.Ensures(ValueSize > 0);

				throw new NotImplementedException();
			}
		}
		public bool Compare(byte[] data, int index, out ScanResult result)
		{
			Contract.Requires(data != null);
			Contract.Requires(index >= 0);

			throw new NotImplementedException();
		}

		public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
		{
			Contract.Requires(data != null);
			Contract.Requires(index >= 0);
			Contract.Requires(previous != null);

			throw new NotImplementedException();
		}
	}
}
