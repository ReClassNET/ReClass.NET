using System.Diagnostics.Contracts;

namespace ReClassNET.Util
{
	public static class HexadecimalFormatter
	{
		private static readonly uint[] lookup = CreateHexLookup();

		private static uint[] CreateHexLookup()
		{
			var result = new uint[256];
			for (var i = 0; i < 256; i++)
			{
				var s = i.ToString("X2");
				result[i] = (uint)s[0] + ((uint)s[1] << 16);
			}
			return result;
		}

		public static string ToString(byte[] data)
		{
			Contract.Requires(data != null);

			if (data.Length == 0)
			{
				return string.Empty;
			}

			var result = new char[data.Length * 2 + data.Length - 1];

			var val = lookup[data[0]];
			result[0] = (char)val;
			result[1] = (char)(val >> 16);

			for (var i = 1; i < data.Length; i++)
			{
				val = lookup[data[i]];
				result[3 * i - 1] = ' ';
				result[3 * i] = (char)val;
				result[3 * i + 1] = (char)(val >> 16);
			}

			return new string(result);
		}
	}
}
