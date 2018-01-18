using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.UI
{
	public class BindingDisplayWrapper<T>
	{
		public T Value { get; }
		private readonly Func<T, string> toString;

		public BindingDisplayWrapper(T value, Func<T, string> toString)
		{
			Contract.Requires(toString != null);

			Value = value;
			this.toString = toString;
		}

		public override string ToString() => toString(Value);
	}
}
