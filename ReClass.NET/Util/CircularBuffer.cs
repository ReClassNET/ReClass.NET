using System;
using System.Collections;
using System.Collections.Generic;

namespace ReClassNET.Util
{
	/// <summary>
	/// A circular buffer with a fixed size.
	/// </summary>
	public class CircularBuffer<T> : IEnumerable<T>
	{
		private readonly T[] buffer;
		private int head;
		private int tail;

		public CircularBuffer(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity));
			}

			buffer = new T[capacity];
			head = capacity - 1;
		}

		public int Count { get; private set; }

		public int Capacity => buffer.Length;

		public T Head => buffer[head];
		public T Tail => buffer[tail];

		public T Enqueue(T item)
		{
			head = (head + 1) % Capacity;
			var overwritten = buffer[head];
			buffer[head] = item;
			if (Count == Capacity)
			{
				tail = (tail + 1) % Capacity;
			}
			else
			{
				++Count;
			}

			return overwritten;
		}

		public T Dequeue()
		{
			if (Count == 0)
			{
				throw new InvalidOperationException();
			}

			var dequeued = buffer[head];
			buffer[head] = default(T);
			if (head == 0)
			{
				head = Capacity - 1;
			}
			else
			{
				head = (head - 1) % Capacity;
			}
			--Count;
			return dequeued;
		}

		public void Clear()
		{
			head = Capacity - 1;
			tail = 0;
			Count = 0;
		}

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				return buffer[(tail + index) % Capacity];
			}
			set
			{
				if (index < 0 || index >= Count)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				buffer[(tail + index) % Capacity] = value;
			}
		}

		public int IndexOf(T item)
		{
			for (var i = 0; i < Count; ++i)
			{
				if (Equals(item, this[i]))
				{
					return i;
				}
			}

			return -1;
		}

		public void Insert(int index, T item)
		{
			if (index < 0 || index > Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (Count == index)
			{
				Enqueue(item);
			}
			else
			{
				var last = this[Count - 1];
				for (var i = index; i < Count - 2; ++i)
				{
					this[i + 1] = this[i];
				}

				this[index] = item;
				Enqueue(last);
			}
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			for (var i = index; i > 0; --i)
			{
				this[i] = this[i - 1];
			}

			Dequeue();
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (Count == 0 || Capacity == 0)
			{
				yield break;
			}

			for (var i = 0; i < Count; ++i)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
