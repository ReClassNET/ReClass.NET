using System.ComponentModel;

namespace ReClassNET.MemoryScanner
{
	public enum ScanValueType
	{
		[Description("Byte")]
		Byte,
		[Description("Short (2 Bytes)")]
		Short,
		[Description("Integer (4 Bytes)")]
		Integer,
		[Description("Long (8 Bytes)")]
		Long,
		[Description("Float (4 Bytes)")]
		Float,
		[Description("Double (8 Bytes)")]
		Double,
		[Description("Array of Bytes")]
		ArrayOfBytes,
		[Description("String")]
		String
	}
}
