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
			const int BitsPerBlock = 4;

			var sb = new StringBuilder(bits);

			var padding = bits - value.Length;

			// Add full padding blocks.
			while (padding > BitsPerBlock)
			{
				sb.Append("0000 ");
				padding -= BitsPerBlock;
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
				sb.Append(value, 0, BitsPerBlock - padding);

				if (value.Length > padding)
				{
					sb.Append(' ');
				}
			}

			// Add all remaining blocks.
			for (var i = padding == 0 ? 0 : BitsPerBlock - padding; i < value.Length; i += BitsPerBlock)
			{
				sb.Append(value, i, BitsPerBlock);
				if (i < value.Length - BitsPerBlock)
				{
					sb.Append(' ');
				}
			}

			return sb.ToString();
		}
	}
}
