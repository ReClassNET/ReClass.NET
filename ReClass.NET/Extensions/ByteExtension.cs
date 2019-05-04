using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.Extensions
{
	public static class ByteExtension
	{
		/// <summary>
		/// Sets every element in the array to zero.
		/// </summary>
		/// <param name="array"></param>
		[DebuggerStepThrough]
		public static void FillWithZero(this byte[] array)
		{
			Contract.Requires(array != null);

			Array.Clear(array, 0, array.Length);
		}
	}
}
