using System;
using System.Text;

namespace ReClassNET.Util
{
	public static class BitString
	{
		/// <summary>
		/// Converts the value to the corresponding bit string.
		/// Format: 0000 0000
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The corresponding bit string.</returns>
		public static string ToString(byte value)
		{
			return AddPaddingAndBuildBlocks(8, Convert.ToString(value, 2));
		}

		/// <summary>
		/// Converts the value to the corresponding bit string.
		/// Format: 0000 0000 0000 0000
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The corresponding bit string.</returns>
		public static string ToString(short value)
		{
			return AddPaddingAndBuildBlocks(16, Convert.ToString(value, 2));
		}

		/// <summary>
		/// Converts the value to the corresponding bit string.
		/// Format: 0000 0000 0000 0000 0000 0000 0000 0000
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The corresponding bit string.</returns>
		public static string ToString(int value)
		{
			return AddPaddingAndBuildBlocks(32, Convert.ToString(value, 2));
		}

		/// <summary>
		/// Converts the value to the corresponding bit string.
		/// Format: 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The corresponding bit string.</returns>
		public static string ToString(long value)
		{
			return AddPaddingAndBuildBlocks(64, Convert.ToString(value, 2));
		}

		private static string AddPaddingAndBuildBlocks(int bits, string value)
		{
			var sb = new StringBuilder(bits);

			var padding = bits - value.Length;

			// Add full padding blocks.
			while (padding > 4)
			{
				sb.Append("0000 ");
				padding -= 4;
			}

			// Add only a part of a block.
			if (padding > 0)
			{
				// {padding} 0 bits
				for (var i = 0; i < padding; ++i)
				{
					sb.Append('0');
				}

				// and {4 - padding} bits of the value.
				sb.Append(value, 0, 4 - padding);

				if (value.Length > padding)
				{
					sb.Append(' ');
				}
			}

			// Add all remaining blocks.
			for (var i = padding == 0 ? 0 : 4 - padding; i < value.Length; i += 4)
			{
				sb.Append(value, i, 4);
				if (i < value.Length - 4)
				{
					sb.Append(' ');
				}
			}

			return sb.ToString();
		}
	}
}
