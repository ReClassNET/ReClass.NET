using System;

namespace ReClassNET.Util.Conversion
{
	public sealed class BigEndianBitConverter : EndianBitConverter
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

			long ret = 0;
			for (var i = 0; i < bytesToConvert; i++)
			{
				ret = unchecked((ret << 8) | buffer[index + i]);
			}
			return ret;
		}

		protected override byte[] ToBytes(long value, int bytes)
		{
			var endOffset = bytes - 1;

			var buffer = new byte[bytes];
			for (var i = 0; i < bytes; i++)
			{
				buffer[endOffset - i] = unchecked((byte)(value & 0xFF));
				value >>= 8;
			}
			return buffer;
		}
	}
}
