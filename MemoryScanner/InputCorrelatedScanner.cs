using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Input;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner.Comparer;

namespace ReClassNET.MemoryScanner
{
	public class InputCorrelatedScanner : Scanner
	{
		private readonly KeyboardInput input;
		private readonly List<KeyboardHotkey> hotkeys;

		public int ScanCount { get; private set; }

		public InputCorrelatedScanner(RemoteProcess process, KeyboardInput input, IEnumerable<KeyboardHotkey> hotkeys, ScanValueType valueType)
			: base(process, CreateScanSettings(valueType))
		{
			Contract.Requires(process != null);
			Contract.Requires(input != null);
			Contract.Requires(hotkeys != null);
			Contract.Ensures(this.input != null);

			this.input = input;
			this.hotkeys = hotkeys.ToList();
		}

		private static ScanSettings CreateScanSettings(ScanValueType valueType)
		{
			Contract.Ensures(Contract.Result<ScanSettings>() != null);

			var settings = ScanSettings.Default;
			settings.ValueType = valueType;
			return settings;
		}

		private IScanComparer CreateScanComparer(ScanCompareType compareType)
		{
			Contract.Ensures(Contract.Result<IScanComparer>() != null);

			switch (Settings.ValueType)
			{
				case ScanValueType.Byte:
					return new ByteMemoryComparer(compareType, 0, 0);
				case ScanValueType.Short:
					return new ShortMemoryComparer(compareType, 0, 0);
				case ScanValueType.Integer:
					return new IntegerMemoryComparer(compareType, 0, 0);
				case ScanValueType.Long:
					return new LongMemoryComparer(compareType, 0, 0);
				case ScanValueType.Float:
					return new FloatMemoryComparer(compareType, ScanRoundMode.Normal, 2, 0, 0);
				case ScanValueType.Double:
					return new DoubleMemoryComparer(compareType, ScanRoundMode.Normal, 2, 0, 0);
				default:
					throw new InvalidOperationException();
			}
		}

		public Task Initialize()
		{
			return Search(CreateScanComparer(ScanCompareType.Unknown), CancellationToken.None, null);
		}

		public async Task CorrelateInput(CancellationToken ct, IProgress<int> progress)
		{
			var keys = input.GetPressedKeys().Select(k => k & Keys.KeyCode).Where(k => k != Keys.None).ToArray();

			var compareType = keys.Length != 0 && hotkeys.Any(h => h.Matches(keys)) ? ScanCompareType.Changed : ScanCompareType.NotChanged;

			await Search(CreateScanComparer(compareType), ct, progress);

			ScanCount++;
		}
	}
}
