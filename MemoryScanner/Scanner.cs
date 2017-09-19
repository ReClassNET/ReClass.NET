using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher
{
	public class Scanner : IDisposable
	{
		private readonly RemoteProcess process;
		private ScanResultStore store;

		public ScanSettings Settings { get; }

		/// <summary>
		/// Gets the total result count from the last scan.
		/// </summary>
		public int TotalResultCount => store.TotalResultCount;

		private bool isFirstScan;

		public Scanner(RemoteProcess process, ScanSettings settings)
		{
			Contract.Requires(process != null);
			Contract.Requires(settings != null);

			this.process = process;
			Settings = settings;

			isFirstScan = true;
		}

		public void Dispose()
		{
			store?.Dispose();
			store = null;
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

			return store.GetResultBlocks().SelectMany(kv => kv.Results);
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
				.Where(s => s.Start.InRange(Settings.StartAddress, Settings.StopAddress))
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
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <param name="progress">The <see cref="IProgress{T}"/> object to report the current progress.</param>
		/// <returns> The asynchronous result indicating if the scan completed.</returns>
		public Task<bool> Search(IScanComparer comparer, CancellationToken ct, IProgress<int> progress)
		{
			return isFirstScan ? FirstScan(comparer, ct, progress) : NextScan(comparer, ct, progress);
		}

		/// <summary>
		/// Starts an async first scan with the provided <see cref="IScanComparer"/>.
		/// </summary>
		/// <param name="comparer">The comparer to scan for values.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <param name="progress">The <see cref="IProgress{T}"/> object to report the current progress.</param>
		/// <returns> The asynchronous result indicating if the scan completed.</returns>
		private Task<bool> FirstScan(IScanComparer comparer, CancellationToken ct, IProgress<int> progress)
		{
			Contract.Requires(comparer != null);
			Contract.Ensures(Contract.Result<Task<bool>>() != null);

			store = CreateStore();

			var sections = GetSearchableSections();

			var initialBufferSize = (int)sections.Average(s => s.Size.ToInt32());

			progress?.Report(0);

			var counter = 0;
			var totalSectionCount = (float)sections.Count;

			return Task.Run(() =>
			{
				// Algorithm:
				// 1. Partition the sections for the worker threads.
				// 2. Create a ScannerContext per worker thread.
				// 3. n Worker -> m Sections: Read data, search results, store results

				var result = Parallel.ForEach(
					sections, // Sections get grouped by the framework to balance the workers.
					new ParallelOptions { CancellationToken = ct},
					() => new ScannerContext(Settings, comparer, initialBufferSize), // Create a new context for every worker (thread).
					(s, state, _, context) =>
					{
						var size = s.Size.ToInt32();
						context.EnsureBufferSize(size);
						var buffer = context.Buffer;
						if (process.ReadRemoteMemoryIntoBuffer(s.Start, ref buffer, 0, size)) // Fill the buffer.
						{
							var results = context.Worker.Search(buffer, size) // Search for results.
								.Select(r => { r.Address = r.Address.Add(s.Start); return r; }) // Results are relative to the buffer so add the section start address.
								.ToList();
							if (results.Count > 0)
							{
								// Minify the result block range.
								var block = new ScanResultBlock(
									results.Min(r => r.Address, IntPtrComparer.Instance),
									results.Max(r => r.Address, IntPtrComparer.Instance) + comparer.ValueSize,
									results
								);
								store.AddBlock(block); // Store the result block.
							}
						}

						progress?.Report((int)(Interlocked.Increment(ref counter) / totalSectionCount * 100));

						return context;
					},
					w => { }
				);

				store.Finish();

				if (result.IsCompleted)
				{
					isFirstScan = false;

					return true;
				}

				return false;
			}, ct);
		}

		/// <summary>
		/// Starts an async next scan with the provided <see cref="IScanComparer"/>.
		/// The next scan uses the previous results to refine the results.
		/// </summary>
		/// <param name="comparer">The comparer to scan for values.</param>
		/// <param name="ct">The <see cref="CancellationToken"/> to stop the scan.</param>
		/// <param name="progress">The <see cref="IProgress{T}"/> object to report the current progress.</param>
		/// <returns> The asynchronous result indicating if the scan completed.</returns>
		private Task<bool> NextScan(IScanComparer comparer, CancellationToken ct, IProgress<int> progress)
		{
			Contract.Requires(comparer != null);
			Contract.Ensures(Contract.Result<Task<bool>>() != null);

			var localStore = CreateStore();

			progress?.Report(0);

			var counter = 0;
			var totalResultCount = (float)store.TotalResultCount;

			return Task.Run(() =>
			{
				var result = Parallel.ForEach(
					store.GetResultBlocks(),
					new ParallelOptions { CancellationToken = ct },
					() => new ScannerContext(Settings, comparer, 0),
					(b, state, _, context) =>
					{
						context.EnsureBufferSize(b.Size);
						var buffer = context.Buffer;
						if (process.ReadRemoteMemoryIntoBuffer(b.Start, ref buffer, 0, b.Size))
						{
							var results = context.Worker.Search(buffer, buffer.Length, b.Results.Select(r => { r.Address = r.Address.Sub(b.Start); return r; }))
								.Select(r => { r.Address = r.Address.Add(b.Start); return r; })
								.ToList();
							if (results.Count > 0)
							{
								var block = new ScanResultBlock(
									results.Min(r => r.Address, IntPtrComparer.Instance),
									results.Max(r => r.Address, IntPtrComparer.Instance) + comparer.ValueSize,
									results
								);
								localStore.AddBlock(block);
							}
						}

						progress?.Report((int)(Interlocked.Add(ref counter, b.Results.Count) / totalResultCount * 100));

						return context;
					},
					w => { }
				);

				localStore.Finish();

				if (result.IsCompleted)
				{
					store.Dispose();

					store = localStore;

					return true;
				}

				return false;
			}, ct);
		}
	}
}
