using System;

namespace ReClassNET.MemorySearcher
{
	public enum SettingState
	{
		Yes,
		No,
		Indeterminate
	}

	public class ScanSettings
	{
		public IntPtr StartAddress { get; set; } = IntPtr.Zero;
		public IntPtr StopAddress { get; set; } =
#if RECLASSNET64
			(IntPtr)long.MaxValue;
#else
			(IntPtr)int.MaxValue;
#endif
		public SettingState SearchWritableMemory { get; set; } = SettingState.Yes;
		public SettingState SearchExecutableMemory { get; set; } = SettingState.Indeterminate;
		public SettingState SearchCopyOnWriteMemory { get; set; } = SettingState.No;
		public bool SearchMemPrivate { get; set; } = true;
		public bool SearchMemImage { get; set; } = true;
		public bool SearchMemMapped { get; set; } = false;
		public bool FastScan { get; set; } = true;
		public int FastScanAlignment { get; set; } = 1;
		public ScanValueType ValueType { get; set; } = ScanValueType.Byte;
	}
}
