using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Project
{
	public class EnumDescription
	{
		public enum UnderlyingTypeSize
		{
			OneByte = 1,
			TwoBytes = 2,
			FourBytes = 4,
			EightBytes = 8
		}

		public static EnumDescription Default => new EnumDescription { Name = "DummyEnum" };

		public string Name { get; set; } = string.Empty;

		public bool UseFlagsMode { get; private set; }

		public UnderlyingTypeSize Size { get; private set; } = UnderlyingTypeSize.FourBytes;

		public IReadOnlyList<KeyValuePair<string, long>> Values { get; private set; } = new Dictionary<string, long>().ToList();

		public void SetData(bool useFlagsMode, UnderlyingTypeSize size, IEnumerable<KeyValuePair<string, long>> values)
		{
			var temp = values.OrderBy(t => t.Value).ToList();

			if (useFlagsMode)
			{
				var maxPossibleValue = ulong.MaxValue;
				switch (size)
				{
					case UnderlyingTypeSize.OneByte:
						maxPossibleValue = byte.MaxValue;
						break;
					case UnderlyingTypeSize.TwoBytes:
						maxPossibleValue = ushort.MaxValue;
						break;
					case UnderlyingTypeSize.FourBytes:
						maxPossibleValue = uint.MaxValue;
						break;
				}

				if (temp.Select(kv => (ulong)kv.Value).Max() > maxPossibleValue)
				{
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				var minPossibleValue = long.MinValue;
				var maxPossibleValue = long.MaxValue;
				switch (size)
				{
					case UnderlyingTypeSize.OneByte:
						minPossibleValue = sbyte.MinValue;
						maxPossibleValue = sbyte.MaxValue;
						break;
					case UnderlyingTypeSize.TwoBytes:
						minPossibleValue = short.MinValue;
						maxPossibleValue = short.MaxValue;
						break;
					case UnderlyingTypeSize.FourBytes:
						minPossibleValue = int.MinValue;
						maxPossibleValue = int.MaxValue;
						break;
				}

				if (temp.Max(kv => kv.Value) > maxPossibleValue || temp.Min(kv => kv.Value) < minPossibleValue)
				{
					throw new ArgumentOutOfRangeException();
				}
			}

			UseFlagsMode = useFlagsMode;
			Size = size;
			Values = temp;
		}
	}
}
