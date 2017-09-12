using System;
using System.Collections.Generic;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher
{
	internal class SearchResultBlock
	{
		public IntPtr Start { get; }
		public IntPtr End { get; }
		public int Size => End.Sub(Start).ToInt32();
		public IReadOnlyList<SearchResult> Results { get; }

		public SearchResultBlock(IntPtr start, IntPtr end, IReadOnlyList<SearchResult> results)
		{
			Start = start;
			End = end;
			Results = results;
		}
	}
}
