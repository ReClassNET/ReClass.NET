using System;
using System.Linq;
using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class CircularBufferTest
	{
		[Fact]
		public void CheckInitialCapacityCount()
		{
			const int Capacity = 5;

			var cb = new CircularBuffer<int>(Capacity);

			Check.That(cb.Capacity).IsEqualTo(Capacity);
			Check.That(cb.Count).IsEqualTo(0);
		}

		[Fact]
		public void DequeueFromEmptyBufferThrows()
		{
			var cb = new CircularBuffer<int>(1);

			Check.ThatCode(() => cb.Dequeue()).Throws<InvalidOperationException>();
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(10)]
		[InlineData(100)]
		public void CheckEnqueueAndDeque(int value)
		{
			var cb = new CircularBuffer<int>(1);

			var overwrittenValue = cb.Enqueue(value);

			Check.That(overwrittenValue).IsEqualTo(default);

			Check.That(cb.Dequeue()).IsEqualTo(value);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(1, 2)]
		[InlineData(1, 2, 3)]
		[InlineData(1, 2, 3, 4)]
		[InlineData(1, 2, 3, 4, 5)]
		public void CheckMultipleEnqueueAndDeque(params int[] values)
		{
			var cb = new CircularBuffer<int>(values.Length);

			foreach (var value in values)
			{
				cb.Enqueue(value);
			}

			foreach (var value in values.Reverse())
			{
				Check.That(cb.Dequeue()).IsEqualTo(value);
			}
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(1, 2, 3)]
		[InlineData(1, 2, 3, 4)]
		[InlineData(1, 2, 3, 4, 5)]
		public void CheckOverflow(params int[] values)
		{
			var cb = new CircularBuffer<int>(1);

			cb.Enqueue(values[0]);

			for (var i = 1; i < values.Length; ++i)
			{
				Check.That(cb.Enqueue(values[i])).IsEqualTo(values[i - 1]);
			}

			Check.That(cb.Dequeue()).IsEqualTo(values[values.Length - 1]);
		}
	}
}
