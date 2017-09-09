using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReClassNET.Memory;
using ReClassNET.MemorySearcher.Algorithm;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher
{
	public class Searcher
	{
		private readonly RemoteProcess process;

		public Searcher(RemoteProcess process)
		{
			Contract.Requires(process != null);

			this.process = process;
		}

		private ICollection<Section> GetSearchableSections(SearchSettings settings)
		{
			Contract.Requires(settings != null);

			return process.Sections
				.Where(s => !s.Protection.HasFlag(SectionProtection.Guard))
				.Where(s => s.Start.InRange(settings.StartAddress, settings.StopAddress))
				.Where(s =>
				{
					switch (s.Type)
					{
						case SectionType.Private: return settings.SearchMemPrivate;
						case SectionType.Image: return settings.SearchMemImage;
						case SectionType.Mapped: return settings.SearchMemMapped;
						default: return false;
					}
				})
				.Where(s =>
				{
					var isWritable = s.Protection.HasFlag(SectionProtection.Write);
					switch (settings.SearchWritableMemory)
					{
						case SettingState.Yes: return isWritable;
						case SettingState.No: return !isWritable;
						default: return true;
					}
				})
				.Where(s =>
				{
					var isExecutable = s.Protection.HasFlag(SectionProtection.Execute);
					switch (settings.SearchExecutableMemory)
					{
						case SettingState.Yes: return isExecutable;
						case SettingState.No: return !isExecutable;
						default: return true;
					}
				})
				.Where(s =>
				{
					var isCopyOnWrite = s.Protection.HasFlag(SectionProtection.CopyOnWrite);
					switch (settings.SearchCopyOnWriteMemory)
					{
						case SettingState.Yes: return isCopyOnWrite;
						case SettingState.No: return !isCopyOnWrite;
						default: return true;
					}
				})
				.ToList();
		}

		public IList<IntPtr> Search(SearchSettings settings, IPatternMatcher matcher, CancellationToken ct, IProgress<int> progress)
		{
			Contract.Requires(settings != null);
			Contract.Requires(matcher != null);

			var sections = GetSearchableSections(settings);

			progress?.Report(0);

			var counter = 0;

			return sections
				.AsParallel()
				.WithCancellation(ct)
				.Select(s =>
				{
					var buffer = new MemoryBuffer(s.Size.ToInt32()) { Process = process };
					buffer.Update(s.Start, false);
					return new { StartAddress = s.Start, Buffer = buffer };
				})
				.SelectMany(i =>
				{
					var result = matcher.SearchMatches(i.Buffer.RawData).Select(offset => i.StartAddress + offset).ToList();
					progress?.Report((int)(Interlocked.Increment(ref counter) / (float)sections.Count * 100));
					return result;
				})
				.ToList();
		}
	}
}
