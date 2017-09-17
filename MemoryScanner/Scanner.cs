using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReClassNET.Memory;
using ReClassNET.MemorySearcher.Comparer;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher
{
	public class Scanner : IDisposable
	{
		private readonly RemoteProcess process;
		private ScanResultStore store;

		public ScanSettings Settings { get; }

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

		public IEnumerable<ScanResult> GetResults()
		{
			Contract.Ensures(Contract.Result<IEnumerable<ScanResult>>() != null);

			return store.GetResultBlocks().SelectMany(kv => kv.Results);
		}

		private ScanResultStore CreateStore()
		{
			return new ScanResultStore(Settings.ValueType, Path.GetTempPath());
		}

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
						case SectionType.Private: return Settings.ScanMemPrivate;
						case SectionType.Image: return Settings.ScanMemImage;
						case SectionType.Mapped: return Settings.ScanMemMapped;
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

		public Task<bool> Search(IScanComparer comparer, CancellationToken ct, IProgress<int> progress)
		{
			return isFirstScan ? FirstScan(comparer, ct, progress) : NextScan(comparer, ct, progress);
		}

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
				var result = Parallel.ForEach(
					sections,
					new ParallelOptions { CancellationToken = ct},
					() => new ScanContext(Settings, comparer, initialBufferSize),
					(s, state, _, context) =>
					{
						var size = s.Size.ToInt32();
						context.EnsureBufferSize(size);
						var buffer = context.Buffer;
						if (process.ReadRemoteMemoryIntoBuffer(s.Start, ref buffer, 0, size))
						{
							var results = context.Worker.Search(buffer, size)
								.Select(r => { r.Address = r.Address.Add(s.Start); return r; })
								.ToList();
							if (results.Count > 0)
							{
								var block = new ScanResultBlock(
									results.Min(r => r.Address, IntPtrComparer.Instance),
									results.Max(r => r.Address, IntPtrComparer.Instance) + comparer.ValueSize,
									results
								);
								store.AddBlock(block);
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
					() => new ScanContext(Settings, comparer, 0),
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
