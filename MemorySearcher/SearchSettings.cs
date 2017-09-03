using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.MemorySearcher
{
	public enum SettingState
	{
		Yes,
		No,
		Indeterminate
	}

	public class SearchSettings
	{
		public IntPtr StartAddress { get; set; } = IntPtr.Zero;
		public IntPtr StopAddress { get; set; } =
#if WIN64
			(IntPtr)long.MaxValue;
#else
			(IntPtr)int.MaxValue;
#endif
		public SettingState SearchOnlyWritableMemory { get; set; } = SettingState.Yes;
		public SettingState SearchOnlyExecutableMemory { get; set; } = SettingState.Indeterminate;
		public SettingState SearchOnlyCopyOnWriteMemory { get; set; } = SettingState.No;
		public bool SearchMemPrivate { get; set; } = true;
		public bool SearchMemImage { get; set; } = true;
		public bool SearchMemMapped { get; set; } = false;
		public bool FastScan { get; set; } = true;
		public int Alignment { get; set; } = IntPtr.Size;

		public static SearchSettings Default => new SearchSettings();
	}
}
