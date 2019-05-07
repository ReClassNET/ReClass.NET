using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using ReClassNET.Extensions;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner
{
	/// <summary>
	/// The store of all found scan results. If the result count exceed the <see cref="MaximumMemoryResultsCount"/> limit,
	/// the results are stored in temporary files.
	/// </summary>
	internal class ScanResultStore : IDisposable
	{
		private enum StorageMode
		{
			Memory,
			File
		}

		private const int MaximumMemoryResultsCount = 10000000;

		private readonly List<ScanResultBlock> store = new List<ScanResultBlock>();

		private readonly string storePath;
		private FileStream fileStream;

		private StorageMode mode = StorageMode.Memory;

		private readonly ScanValueType valueType;

		/// <summary>
		/// Gets the number of total results.
		/// </summary>
		public int TotalResultCount { get; private set; }

		public ScanResultStore(ScanValueType valueType, string storePath)
		{
			this.valueType = valueType;
			this.storePath = Path.Combine(storePath, $"ReClass.NET_MemoryScanner_{Guid.NewGuid()}.tmp");
		}

		public void Dispose()
		{
			Finish();

			store.Clear();

			try
			{
				if (File.Exists(storePath))
				{
					File.Delete(storePath);
				}
			}
			catch
			{
				// ignored
			}
		}

		public void Finish()
		{
			if (mode == StorageMode.File)
			{
				fileStream?.Dispose();
				fileStream = null;
			}
		}

		/// <summary>
		/// Gets the result blocks from the store. This may read results from files..
		/// </summary>
		public IEnumerable<ScanResultBlock> GetResultBlocks()
		{
			Contract.Ensures(Contract.Result<IEnumerable<ScanResultBlock>>() != null);

			return mode == StorageMode.Memory ? store : ReadBlocksFromFile();
		}

		/// <summary>
		/// Adds a result block to the store. If the result count exceed the <see cref="MaximumMemoryResultsCount"/> limit,
		/// the results are stored in temporary files.
		/// </summary>
		/// <param name="block">The result block to add.</param>
		public void AddBlock(ScanResultBlock block)
		{
			Contract.Requires(block != null);

			lock (store)
			{
				TotalResultCount += block.Results.Count;

				if (mode == StorageMode.Memory)
				{
					if (TotalResultCount > MaximumMemoryResultsCount)
					{
						mode = StorageMode.File;

						fileStream = File.OpenWrite(storePath);

						foreach (var b in store)
						{
							AppendBlockToFile(b);
						}
						store.Clear();
						store.TrimExcess();

						AppendBlockToFile(block);
					}
					else
					{
						store.Add(block);
					}
				}
				else
				{
					AppendBlockToFile(block);
				}
			}
		}

		/// <summary>
		/// Writes a result block to the file.
		/// </summary>
		/// <param name="block">The result block to add.</param>
		private void AppendBlockToFile(ScanResultBlock block)
		{
			Contract.Requires(block != null);

			using (var bw = new BinaryWriter(fileStream, Encoding.Unicode, true))
			{
				bw.Write(block.Start);
				bw.Write(block.End);
				bw.Write(block.Results.Count);

				foreach (var result in block.Results)
				{
					WriteSearchResult(bw, result);
				}
			}
		}

		/// <summary>
		/// Reads all memory blocks from the file.
		/// </summary>
		private IEnumerable<ScanResultBlock> ReadBlocksFromFile()
		{
			Contract.Ensures(Contract.Result<IEnumerable<ScanResultBlock>>() != null);

			using (var stream = File.OpenRead(storePath))
			{
				using (var br = new BinaryReader(stream, Encoding.Unicode))
				{
					var length = stream.Length;

					while (stream.Position < length)
					{
						var start = br.ReadIntPtr();
						var end = br.ReadIntPtr();

						var resultCount = br.ReadInt32();

						var results = new List<ScanResult>(resultCount);
						for (var i = 0; i < resultCount; ++i)
						{
							results.Add(ReadScanResult(br));
						}

						yield return new ScanResultBlock(start, end, results);
					}
				}
			}
		}

		/// <summary>
		/// Reads a single scan result from the file.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the <see cref="ScanValueType"/> is not valid.</exception>
		/// <param name="br">The <see cref="BinaryReader"/> to read from.</param>
		/// <returns>The scan result.</returns>
		private ScanResult ReadScanResult(BinaryReader br)
		{
			Contract.Ensures(Contract.Result<ScanResult>() != null);

			var address = br.ReadIntPtr();

			ScanResult result;
			switch (valueType)
			{
				case ScanValueType.Byte:
					result = new ByteScanResult(br.ReadByte());
					break;
				case ScanValueType.Short:
					result = new ShortScanResult(br.ReadInt16());
					break;
				case ScanValueType.Integer:
					result = new IntegerScanResult(br.ReadInt32());
					break;
				case ScanValueType.Long:
					result = new LongScanResult(br.ReadInt64());
					break;
				case ScanValueType.Float:
					result = new FloatScanResult(br.ReadSingle());
					break;
				case ScanValueType.Double:
					result = new DoubleScanResult(br.ReadDouble());
					break;
				case ScanValueType.ArrayOfBytes:
					result = new ArrayOfBytesScanResult(br.ReadBytes(br.ReadInt32()));
					break;
				case ScanValueType.String:
					var encoding = br.ReadInt32();
					result = new StringScanResult(br.ReadString(), encoding == 0 ? Encoding.UTF8 : encoding == 1 ? Encoding.Unicode : Encoding.UTF32);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			result.Address = address;

			return result;
		}

		/// <summary>
		/// Writes a single scan result to the file.
		/// </summary>
		/// <param name="bw">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="result">The result to write.</param>
		private static void WriteSearchResult(BinaryWriter bw, ScanResult result)
		{
			Contract.Requires(bw != null);
			Contract.Requires(result != null);

			bw.Write(result.Address);

			switch (result)
			{
				case ByteScanResult byteSearchResult:
					bw.Write(byteSearchResult.Value);
					break;
				case ShortScanResult shortSearchResult:
					bw.Write(shortSearchResult.Value);
					break;
				case IntegerScanResult integerSearchResult:
					bw.Write(integerSearchResult.Value);
					break;
				case LongScanResult longSearchResult:
					bw.Write(longSearchResult.Value);
					break;
				case FloatScanResult floatSearchResult:
					bw.Write(floatSearchResult.Value);
					break;
				case DoubleScanResult doubleSearchResult:
					bw.Write(doubleSearchResult.Value);
					break;
				case ArrayOfBytesScanResult arrayOfBytesSearchResult:
					bw.Write(arrayOfBytesSearchResult.Value.Length);
					bw.Write(arrayOfBytesSearchResult.Value);
					break;
				case StringScanResult stringSearchResult:
					bw.Write(stringSearchResult.Encoding.IsSameCodePage(Encoding.UTF8) ? 0 : stringSearchResult.Encoding.IsSameCodePage(Encoding.Unicode) ? 1 : 2);
					bw.Write(stringSearchResult.Value);
					break;
			}
		}
	}
}
