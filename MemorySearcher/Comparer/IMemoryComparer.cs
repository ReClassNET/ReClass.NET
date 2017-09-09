namespace ReClassNET.MemorySearcher.Comparer
{
	public interface IMemoryComparer
	{
		SearchCompareType CompareType { get; }
		int ValueSize { get; }

		bool Compare(byte[] data, int index);

		bool Compare(byte[] data, int index, SearchResult other);
	}
}
