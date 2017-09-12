using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher
{
	internal class SearchResultStore : IDisposable
	{
		private enum StorageMode
		{
			Memory,
			File
		}

		private const int MaximumMemoryResultsCount = 10000000;

		private readonly List<SearchResultBlock> store = new List<SearchResultBlock>();

		private readonly string storePath;
		private FileStream fileStream;

		private StorageMode mode = StorageMode.Memory;

		private readonly SearchValueType valueType = SearchValueType.Byte;

		public int TotalResultCount { get; private set; }

		public SearchResultStore(string storePath)
		{
			this.storePath = Path.Combine(storePath, $"ReClass.NET_MemorySearcher_{Guid.NewGuid()}.tmp");
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

		public IEnumerable<SearchResultBlock> GetResultBlocks()
		{
			return mode == StorageMode.Memory ? store : ReadBlocksFromFile();
		}

		public void AddBlock(SearchResultBlock block)
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
				}
				else
				{
					AppendBlockToFile(block);
				}
			}
		}

		private void AppendBlockToFile(SearchResultBlock block)
		{
			Contract.Requires(block != null);

			using (var bw = new BinaryWriter(fileStream, Encoding.ASCII, true))
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

		private IEnumerable<SearchResultBlock> ReadBlocksFromFile()
		{
			using (var stream = File.OpenRead(storePath))
			{
				using (var br = new BinaryReader(stream))
				{
					var length = stream.Length;

					while (stream.Position < length)
					{
						var start = br.ReadIntPtr();
						var end = br.ReadIntPtr();

						var resultCount = br.ReadInt32();

						var results = new List<SearchResult>(resultCount);
						for (var i = 0; i < resultCount; ++i)
						{
							results.Add(ReadSearchResult(br));
						}

						yield return new SearchResultBlock(start, end, results);
					}
				}
			}
		}

		private SearchResult ReadSearchResult(BinaryReader br)
		{
			var address = br.ReadIntPtr();

			SearchResult result;
			switch (valueType)
			{
				case SearchValueType.Byte:
					result = new ByteSearchResult(br.ReadByte());
					break;
				case SearchValueType.Short:
					result = new ShortSearchResult(br.ReadInt16());
					break;
				case SearchValueType.Integer:
					result = new IntegerSearchResult(br.ReadInt32());
					break;
				case SearchValueType.Long:
					result = new LongSearchResult(br.ReadInt64());
					break;
				case SearchValueType.Float:
					result = new FloatSearchResult(br.ReadSingle());
					break;
				case SearchValueType.Double:
					result = new DoubleSearchResult(br.ReadDouble());
					break;
				case SearchValueType.ArrayOfBytes:
					result = new ArrayOfBytesSearchResult();
					break;
				case SearchValueType.String:
					result = new StringSearchResult();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			result.Address = address;

			return result;
		}

		private static void WriteSearchResult(BinaryWriter bw, SearchResult result)
		{
			bw.Write(result.Address);

			switch (result)
			{
				case ByteSearchResult byteSearchResult:
					bw.Write(byteSearchResult.Value);
					break;
				case ShortSearchResult shortSearchResult:
					bw.Write(shortSearchResult.Value);
					break;
				case IntegerSearchResult integerSearchResult:
					bw.Write(integerSearchResult.Value);
					break;
				case LongSearchResult longSearchResult:
					bw.Write(longSearchResult.Value);
					break;
				case FloatSearchResult floatSearchResult:
					bw.Write(floatSearchResult.Value);
					break;
				case DoubleSearchResult doubleSearchResult:
					bw.Write(doubleSearchResult.Value);
					break;
				case ArrayOfBytesSearchResult arrayOfBytesSearchResult:
					
					break;
				case StringSearchResult stringSearchResult:
					
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(result));
			}
		}
	}
}
