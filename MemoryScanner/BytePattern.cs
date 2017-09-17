using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner
{
	public class BytePattern
	{
		private struct PatternByte
		{
			private struct Nibble
			{
				public int Value;
				public bool IsWildcard;
			}

			private Nibble nibble1;
			private Nibble nibble2;

			public bool HasWildcard => nibble1.IsWildcard || nibble2.IsWildcard;

			public byte ByteValue => !HasWildcard ? (byte)(nibble1.Value << 4 + nibble2.Value) : throw new InvalidOperationException();

			private static bool IsHexValue(char c)
			{
				return '0' <= c && c <= '9'
					|| 'A' <= c && c <= 'F'
					|| 'a' <= c && c <= 'f';
			}

			private static int HexToInt(char c)
			{
				if ('0' <= c && c <= '9') return c - '0';
				if ('A' <= c && c <= 'F') return c - 'A' + 10;
				return c - 'a' + 10;
			}

			public bool TryRead(StringReader sr)
			{
				Contract.Requires(sr != null);

				var temp = sr.ReadSkipWhitespaces();
				if (temp == -1 || !IsHexValue((char)temp) && (char)temp != '?')
				{
					return false;
				}

				nibble1.Value = HexToInt((char)temp) & 0xF;
				nibble1.IsWildcard = (char)temp == '?';

				temp = sr.Read();
				if (temp == -1 || char.IsWhiteSpace((char)temp) || (char)temp == '?')
				{
					nibble2.IsWildcard = true;

					return true;
				}

				if (!IsHexValue((char)temp))
				{
					return false;
				}
				nibble2.Value = HexToInt((char)temp) & 0xF;
				nibble2.IsWildcard = false;

				return true;
			}

			public bool Equals(byte b)
			{
				var matched = 0;
				if (nibble1.IsWildcard || ((b >> 4) & 0xF) == nibble1.Value)
				{
					++matched;
				}
				if (nibble2.IsWildcard || (b & 0xF) == nibble2.Value)
				{
					++matched;
				}
				return matched == 2;
			}
		}

		private readonly List<PatternByte> pattern = new List<PatternByte>();

		public int Length => pattern.Count;

		public bool HasWildcards => pattern.Any(pb => pb.HasWildcard);

		public static BytePattern Parse(string value)
		{
			Contract.Requires(!string.IsNullOrEmpty(value));

			var pattern = new BytePattern();

			using (var sr = new StringReader(value))
			{
				var pb = new PatternByte();
				while (pb.TryRead(sr))
				{
					pattern.pattern.Add(pb);
				}

				// Check if we are not at the end of the stream
				if (sr.Peek() != -1)
				{
					throw new ArgumentException();
				}
			}

			return pattern;
		}

		public bool Equals(byte[] data, int index)
		{
			for (var j = 0; j < pattern.Count; ++j)
			{
				if (!pattern[j].Equals(data[index + j]))
				{
					return false;
				}
			}

			return true;
		}

		public byte[] ToByteArray()
		{
			if (HasWildcards)
			{
				throw new InvalidOperationException();
			}

			return pattern.Select(pb => pb.ByteValue).ToArray();
		}
	}
}
