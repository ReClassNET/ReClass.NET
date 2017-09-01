using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Memory;

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

		private IEnumerable<MemoryBuffer> GetSearchableBuffers()
		{
			// exclude range
			// exclude read only

			return process.Sections.Select(s => { var b = new MemoryBuffer { Size = s.Size.ToInt32() }; b.Update(s.Start); return b; });
		}

		public void Search(IPatternMatcher matcher)
		{
			/*var foundMatches = GetSearchableBuffers().AsParallel().SelectMany(mb => matcher.SearchMatches(mb.RawData)).ToList();*/
		}
	}
}
