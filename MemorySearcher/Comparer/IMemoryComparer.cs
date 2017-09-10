namespace ReClassNET.MemorySearcher.Comparer
{
	public interface IMemoryComparer
	{
		SearchCompareType CompareType { get; }
		int ValueSize { get; }

		bool Compare(byte[] data, int index, out SearchResult result);

		bool Compare(byte[] data, int index, SearchResult previous, out SearchResult result);
	}
}
