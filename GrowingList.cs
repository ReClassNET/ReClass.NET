using System;
using System.Collections.Generic;

namespace ReClassNET
{
	public class GrowingList<T>
	{
		private List<T> list;

		public T DefaultValue { get; set; }

		public GrowingList()
		{
			list = new List<T>();
		}

		public GrowingList(T defaultValue)
			: this()
		{
			DefaultValue = defaultValue;
		}

		private void GrowToSize(int size)
		{
			for (var i = list.Count; i <= size; ++i)
			{
				list.Add(DefaultValue);
			}
		}

		private void CheckIndex(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (index >= list.Count)
			{
				GrowToSize(index);
			}
		}

		public T this[int index]
		{
			get
			{
				CheckIndex(index);

				return list[index];
			}
			set
			{
				CheckIndex(index);

				list[index] = value;
			}
		}
	}
}
