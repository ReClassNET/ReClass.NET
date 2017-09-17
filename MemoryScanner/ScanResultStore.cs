using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner
{
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

		public int TotalResultCount { get; private set; }

		public ScanResultStore(ScanValueType valueType, string storePath)
		{
			this.valueType = valueType;
			this.storePath = Path.Combine(storePath, $"ReClass.NET_MemoryScanner_{Guid.NewGuid()}.tmp");
		}

		public void Dispose()
		{
			Finish();

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

		public IEnumerable<ScanResultBlock> GetResultBlocks()
		{
			return mode == StorageMode.Memory ? store : ReadBlocksFromFile();
		}

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

		private IEnumerable<ScanResultBlock> ReadBlocksFromFile()
		{
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

		private ScanResult ReadScanResult(BinaryReader br)
		{
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

		private static void WriteSearchResult(BinaryWriter bw, ScanResult result)
		{
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
					bw.Write(stringSearchResult.Encoding == Encoding.UTF8 ? 0 : stringSearchResult.Encoding == Encoding.Unicode ? 1 : 2);
					bw.Write(stringSearchResult.Value);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(result));
			}
		}
	}
}
