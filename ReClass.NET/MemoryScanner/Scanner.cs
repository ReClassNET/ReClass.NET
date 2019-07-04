using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner
{
	public class Scanner : IDisposable
	{
		/// <summary>
		/// Helper class for consolidated memory regions.
		/// </summary>
		private class ConsolidatedMemoryRegion
		{
			public IntPtr Address { get; set; }
			public int Size { get; set; }
		}

		private readonly RemoteProcess process;
		private readonly CircularBuffer<ScanResultStore> stores;

		public ScanSettings Settings { get; }

		private ScanResultStore CurrentStore => stores.Head;

		/// <summary>
		/// Gets the total result count from the last scan.
		/// </summary>
		public int TotalResultCount => CurrentStore?.TotalResultCount ?? 0;

		/// <summary>
		/// Checks if the last scan can be undone.
		/// </summary>
		public bool CanUndoLastScan => stores.Count > 1;

		private bool isFirstScan;

		public Scanner(RemoteProcess process, ScanSettings settings)
		{
			Contract.Requires(process != null);
			Contract.Requires(settings != null);

			stores = new CircularBuffer<ScanResultStore>(3);

			this.process = process;
			Settings = settings;

			isFirstScan = true;
		}

		public void Dispose()
		{
			foreach (var store in stores)
			{
				store?.Dispose();
			}
			stores.Clear();
		}

		/// <summary>
		/// Retrieves the results of the last scan from the store.
		/// </summary>
		/// <returns>
		/// An enumeration of the <see cref="ScanResult"/>s of the last scan.
		/// </returns>
		public IEnumerable<ScanResult> GetResults()
		{
			Contract.Ensures(Contract.Result<IEnumerable<ScanResult>>() != null);

			if (CurrentStore == null)
			{
				return Enumerable.Empty<ScanResult>();
			}

			return CurrentStore.GetResultBlocks().SelectMany(rb => rb.Results.Select(r =>
			{
				// Convert the block offset to a real address.
				var c = r.Clone();
				c.Address = c.Address.Add(rb.Start);
				return c;
			}));
		}

		/// <summary>
		/// Restores the results of the previous scan.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if no previous results are present.</exception>
		public void UndoLastScan()
		{
			if (!CanUndoLastScan)
			{
				throw new InvalidOperationException();
			}

			var store = stores.Dequeue();
			store?.Dispose();
		}

		/// <summary>
		/// Creates a new <see cref="ScanResultStore"/> and uses the system temporary path as file location.
		/// </summary>
		/// <returns>The new <see cref="ScanResultStore"/>.</returns>
		private ScanResultStore CreateStore()
		{
			return new ScanResultStore(Settings.ValueType, Path.GetTempPath());
		}

		/// <summary>
		/// Gets a list of the sections which meet the provided scan settings.
		/// </summary>
		/// <returns>A list of searchable sections.</returns>
		private IList<Section> GetSearchableSections()
		{
			Contract.Ensures(Contract.Result<IList<Section>>() != null);

			return process.Sections
				.Where(s => !s.Protection.HasFlag(SectionProtection.Guard))
				.Where(s => s.Start.IsInRange(Settings.StartAddress, Settings.StopAddress)
							|| Settings.StartAddress.IsInRange(s.Start, s.End)
							|| Settings.StopAddress.IsInRange(s.Start, s.End))
				.Where(s =>
				{
					switch (s.Type)
					{
						case SectionType.Private: return Settings.ScanPrivateMemory;
						case SectionType.Image: return Settings.ScanImageMemory;
						case SectionType.Mapped: return Settings.ScanMappedMemory;
						default: return false;
					}
				})
				.Where(s =>
				{
					var isWritable = s.Protection.HasFlag(SectionProtection.Write);
					switch (Settings.ScanWritableMemory)
					{
						case SettingState.Yes: return isWritable;
						case SettingState.No: return !isWritable;
						default: return true;
					}
				})
				.Where(s =>
				{
					var isExecutable = s.Protection.HasFlag(SectionProtection.Execute);
					switch (Settings.ScanExecutableMemory)
					{
						case SettingState.Yes: return isExecutable;
						case SettingState.No: return !isExecutable;
						default: return true;
					}
				})
				.Where(s =>
				{
					var isCopyOnWrite = s.Protection.HasFlag(SectionProtection.CopyOnWrite);
					switch (Settings.ScanCopyOnWriteMemory)
					{
						case SettingState.Yes: return isCopyOnWrite;
						case SettingState.No: return !isCopyOnWrite;
						default: return true;
					}
				})
				.ToList();
		}

		/// <summary>
		/// Starts an async search with the provided <see cref="IScanComparer"/>.
		/// The results are stored in the store.
		/// </summary>
		/// <param name="comparer">The comparer to scan for values.</param>
		/// <param name="progress">The <see cref="IProgress{T}"/> object to report the current progress.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <returns> The asynchronous result indicating if the scan completed.</returns>
		public Task<bool> Search(IScanComparer comparer, IProgress<int> progress, CancellationToken ct)
		{
			return isFirstScan ? FirstScan(comparer, progress, ct) : NextScan(comparer, progress, ct);
		}

		/// <summary>
		/// Starts an async first scan with the provided <see cref="IScanComparer"/>.
		/// </summary>
		/// <param name="comparer">The comparer to scan for values.</param>
		/// <param name="progress">The <see cref="IProgress{T}"/> object to report the current progress.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <returns> The asynchronous result indicating if the scan completed.</returns>
		private Task<bool> FirstScan(IScanComparer comparer, IProgress<int> progress, CancellationToken ct)
		{
			Contract.Requires(comparer != null);
			Contract.Ensures(Contract.Result<Task<bool>>() != null);

			var store = CreateStore();

			var sections = GetSearchableSections();
			if (sections.Count == 0)
			{
				return Task.FromResult(true);
			}

			var regions = ConsolidateSections(sections);

			var initialBufferSize = (int)(regions.Average(s => s.Size) + 1);

			progress?.Report(0);

			var counter = 0;
			var totalSectionCount = (float)regions.Count;

			return Task.Run(() =>
			{
				// Algorithm:
				// 1. Partition the sections for the worker threads.
				// 2. Create a ScannerContext per worker thread.
				// 3. n Worker -> m Sections: Read data, search results, store results

				var result = Parallel.ForEach(
					regions, // Sections get grouped by the framework to balance the workers.
					() => new ScannerContext(Settings, comparer, initialBufferSize), // Create a new context for every worker (thread).
					(s, state, _, context) =>
					{
						if (!ct.IsCancellationRequested)
						{
							var start = s.Address;
							var end = s.Address + s.Size;
							var size = s.Size;

							if (Settings.StartAddress.IsInRange(start, end))
							{
								size = size - Settings.StartAddress.Sub(start).ToInt32();
								start = Settings.StartAddress;
							}
							if (Settings.StopAddress.IsInRange(start, end))
							{
								size = size - end.Sub(Settings.StopAddress).ToInt32();
							}

							context.EnsureBufferSize(size);
							var buffer = context.Buffer;
							if (process.ReadRemoteMemoryIntoBuffer(start, ref buffer, 0, size)) // Fill the buffer.
							{
								var results = context.Worker.Search(buffer, size, ct) // Search for results.
									.OrderBy(r => r.Address, IntPtrComparer.Instance)
									.ToList();
								if (results.Count > 0)
								{
									var block = CreateResultBlock(results, start, comparer.ValueSize);
									store.AddBlock(block); // Store the result block.
								}
							}

							progress?.Report((int)(Interlocked.Increment(ref counter) / totalSectionCount * 100));
						}
						else
						{
							state.Stop();
						}
						return context;
					},
					w => { }
				);

				store.Finish();

				var previousStore = stores.Enqueue(store);
				previousStore?.Dispose();

				isFirstScan = false;

				return result.IsCompleted;
			}, ct);
		}

		/// <summary>
		/// Starts an async next scan with the provided <see cref="IScanComparer"/>.
		/// The next scan uses the previous results to refine the results.
		/// </summary>
		/// <param name="comparer">The comparer to scan for values.</param>
		/// <param name="progress">The <see cref="IProgress{T}"/> object to report the current progress.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <returns> The asynchronous result indicating if the scan completed.</returns>
		private Task<bool> NextScan(IScanComparer comparer, IProgress<int> progress, CancellationToken ct)
		{
			Contract.Requires(comparer != null);
			Contract.Ensures(Contract.Result<Task<bool>>() != null);

			var store = CreateStore();

			progress?.Report(0);

			var counter = 0;
			var totalResultCount = (float)CurrentStore.TotalResultCount;

			return Task.Run(() =>
			{
				var result = Parallel.ForEach(
					CurrentStore.GetResultBlocks(),
					() => new ScannerContext(Settings, comparer, 0),
					(b, state, _, context) =>
					{
						if (!ct.IsCancellationRequested)
						{
							context.EnsureBufferSize(b.Size);
							var buffer = context.Buffer;
							if (process.ReadRemoteMemoryIntoBuffer(b.Start, ref buffer, 0, b.Size))
							{
								var results = context.Worker.Search(buffer, buffer.Length, b.Results, ct)
									.OrderBy(r => r.Address, IntPtrComparer.Instance)
									.ToList();
								if (results.Count > 0)
								{
									var block = CreateResultBlock(results, b.Start, comparer.ValueSize);
									store.AddBlock(block);
								}
							}

							progress?.Report((int)(Interlocked.Add(ref counter, b.Results.Count) / totalResultCount * 100));
						}
						else
						{
							state.Stop();
						}
						return context;
					},
					w => { }
				);

				store.Finish();

				var previousStore = stores.Enqueue(store);
				previousStore?.Dispose();

				return result.IsCompleted;
			}, ct);
		}

		/// <summary>
		/// Consolidate memory sections which are direct neighbours to reduce the number of work items.
		/// </summary>
		/// <param name="sections">A list of sections.</param>
		/// <returns>A list of consolidated memory regions.</returns>
		private static List<ConsolidatedMemoryRegion> ConsolidateSections(IList<Section> sections)
		{
			var regions = new List<ConsolidatedMemoryRegion>();

			if (sections.Count > 0)
			{
				var address = sections[0].Start;
				var size = sections[0].Size.ToInt32();

				for (var i = 1; i < sections.Count; ++i)
				{
					var section = sections[i];
					if (address + size != section.Start)
					{
						regions.Add(new ConsolidatedMemoryRegion { Address = address, Size = size });

						address = section.Start;
						size = section.Size.ToInt32();
					}
					else
					{
						size += section.Size.ToInt32();
					}
				}

				regions.Add(new ConsolidatedMemoryRegion { Address = address, Size = size });
			}

			return regions;
		}

		/// <summary>
		/// Creates a result block from the scan results and adjusts the result offset.
		/// </summary>
		/// <param name="results">The results in this block.</param>
		/// <param name="previousStartAddress">The start address of the previous block or section.</param>
		/// <param name="valueSize">The size of the value type.</param>
		/// <returns>The new result block.</returns>
		private static ScanResultBlock CreateResultBlock(IReadOnlyList<ScanResult> results, IntPtr previousStartAddress, int valueSize)
		{
			// Calculate start and end address
			var startAddress = results.First().Address.Add(previousStartAddress);
			var endAddress = results.Last().Address.Add(previousStartAddress) + valueSize;

			// Adjust the offsets of the results
			var firstOffset = results.First().Address;
			foreach (var result in results)
			{
				result.Address = result.Address.Sub(firstOffset);
			}

			var block = new ScanResultBlock(
				startAddress,
				endAddress,
				results
			);
			return block;
		}
	}
}
