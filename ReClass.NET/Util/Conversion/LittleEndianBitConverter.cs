using System;

namespace ReClassNET.Util.Conversion
{
	public sealed class LittleEndianBitConverter : EndianBitConverter
	{
		protected override long FromBytes(byte[] buffer, int index, int bytesToConvert)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}
			if (index + bytesToConvert > buffer.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			var ret = 0L;
			for (var i = 0; i < bytesToConvert; i++)
			{
				ret = unchecked((ret << 8) | buffer[index + bytesToConvert - 1 - i]);
			}
			return ret;
		}

		protected override byte[] ToBytes(long value, int bytes)
		{
			var buffer = new byte[bytes];

			for (var i = 0; i < bytes; i++)
			{
				buffer[i] = unchecked((byte)(value & 0xFF));
				value >>= 8;
			}

			return buffer;
		}
	}
}
