using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Project
{
	public class EnumMetaData
	{
		public static EnumMetaData Default => new EnumMetaData { Name = "DummyEnum" };

		public string Name { get; set; }

		public bool UseFlagsMode { get; private set; }

		public int UnderlyingTypeSize { get; private set; } = sizeof(int);

		public IReadOnlyList<KeyValuePair<long, string>> Values { get; private set; }

		public void SetData(bool useFlagsMode, int underlyingTypeSize, IEnumerable<KeyValuePair<long, string>> values)
		{
			if (!(underlyingTypeSize == 1 || underlyingTypeSize == 2 || underlyingTypeSize == 4 || underlyingTypeSize == 8))
			{
				throw new ArgumentOutOfRangeException(nameof(underlyingTypeSize));
			}
			
			var temp = values.OrderBy(t => t.Key).ToList();

			if (useFlagsMode)
			{
				var maxPossibleValue = ulong.MaxValue;
				switch (underlyingTypeSize)
				{
					case 1:
						maxPossibleValue = byte.MaxValue;
						break;
					case 2:
						maxPossibleValue = ushort.MaxValue;
						break;
					case 4:
						maxPossibleValue = uint.MaxValue;
						break;
				}

				if (temp.Select(kv => (ulong)kv.Key).Max() > maxPossibleValue)
				{
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				var minPossibleValue = long.MinValue;
				var maxPossibleValue = long.MaxValue;
				switch (underlyingTypeSize)
				{
					case 1:
						minPossibleValue = sbyte.MinValue;
						maxPossibleValue = sbyte.MaxValue;
						break;
					case 2:
						minPossibleValue = short.MinValue;
						maxPossibleValue = short.MaxValue;
						break;
					case 4:
						minPossibleValue = int.MinValue;
						maxPossibleValue = int.MaxValue;
						break;
				}

				if (temp.Max(kv => kv.Key) > maxPossibleValue || temp.Min(kv => kv.Key) < minPossibleValue)
				{
					throw new ArgumentOutOfRangeException();
				}
			}

			UseFlagsMode = useFlagsMode;
			Values = temp;
		}
	}
}
