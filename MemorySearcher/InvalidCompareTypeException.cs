using System;

namespace ReClassNET.MemorySearcher
{
	public class InvalidCompareTypeException : Exception
	{
		public InvalidCompareTypeException(ScanCompareType type)
			: base($"{type} is not valid in the current state.")
		{
			
		}
	}
}
