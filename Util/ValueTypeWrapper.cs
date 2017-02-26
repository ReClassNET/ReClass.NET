namespace ReClassNET.Util
{
	/// <summary>A wrapper for non reference types.</summary>
	/// <typeparam name="T">Generic type parameter.</typeparam>
	public class ValueTypeWrapper<T> where T : struct
	{
		public ValueTypeWrapper(T value)
		{
			Value = value;
		}

		public T Value { get; set; }

		public static implicit operator ValueTypeWrapper<T>(T value)
		{
			return new ValueTypeWrapper<T>(value);
		}
	}
}
