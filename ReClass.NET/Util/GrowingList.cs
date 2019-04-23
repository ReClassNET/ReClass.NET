using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.Util
{
	public class GrowingList<T>
	{
		private readonly List<T> list;

		public T DefaultValue { get; set; }

		public int Count => list.Count;

		public GrowingList()
		{
			Contract.Ensures(list != null);

			list = new List<T>();
		}

		public GrowingList(T defaultValue)
			: this()
		{
			DefaultValue = defaultValue;
		}

		private void GrowToSize(int size)
		{
			list.Capacity = size;

			for (var i = list.Count; i <= size; ++i)
			{
				list.Add(DefaultValue);
			}
		}

		private void CheckIndex(int index)
		{
			Contract.Requires(index >= 0);

			if (index >= list.Count)
			{
				GrowToSize(index);
			}
		}

		public T this[int index]
		{
			get
			{
				Contract.Requires(index >= 0);

				CheckIndex(index);

				return list[index];
			}
			set
			{
				Contract.Requires(index >= 0);

				CheckIndex(index);

				list[index] = value;
			}
		}
	}
}
