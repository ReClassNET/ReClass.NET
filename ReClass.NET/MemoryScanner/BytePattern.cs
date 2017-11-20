using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner
{
	public enum PatternFormat
	{
		/// <summary>
		/// Example: AA BB ?? D? ?E FF
		/// </summary>
		Combined,
		/// <summary>
		/// Example: \xAA\xBB\x00\x00\x00\xFF xx???x
		/// </summary>
		PatternAndMask
	}

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

			public byte ByteValue => !HasWildcard ? (byte)((nibble1.Value << 4) + nibble2.Value) : throw new InvalidOperationException();

			public static PatternByte AsByte(byte b)
			{
				var pb = new PatternByte
				{
					nibble1 = { Value = (b >> 4) & 0xF },
					nibble2 = { Value = b & 0xF }
				};
				return pb;
			}

			public static PatternByte AsWildcard()
			{
				var pb = new PatternByte
				{
					nibble1 = { IsWildcard = true },
					nibble2 = { IsWildcard = true }
				};
				return pb;
			}

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

			public Tuple<string, string> ToString(PatternFormat format)
			{
				switch (format)
				{
					case PatternFormat.PatternAndMask:
						return HasWildcard ? Tuple.Create("\\x00", "?") : Tuple.Create($"\\x{ByteValue:X02}", "x");
					case PatternFormat.Combined:
						var sb = new StringBuilder();
						if (nibble1.IsWildcard) sb.Append('?');
						else sb.AppendFormat("{0:X}", nibble1.Value);
						if (nibble2.IsWildcard) sb.Append('?');
						else sb.AppendFormat("{0:X}", nibble2.Value);
						return Tuple.Create<string, string>(sb.ToString(), null);
					default:
						throw new ArgumentOutOfRangeException(nameof(format), format, null);
				}
			}

			public override string ToString() => ToString(PatternFormat.Combined).Item1;
		}

		private readonly List<PatternByte> pattern = new List<PatternByte>();

		/// <summary>
		/// Gets the length of the pattern in byte.
		/// </summary>
		public int Length => pattern.Count;

		/// <summary>
		/// Gets if the pattern contains wildcards.
		/// </summary>
		public bool HasWildcards => pattern.Any(pb => pb.HasWildcard);

		/// <summary>
		/// Parses the provided string for a byte pattern. Wildcards are supported by nibble.
		/// </summary>
		/// <example>
		/// Valid patterns:
		/// AA BB CC DD
		/// AABBCCDD
		/// aabb CCdd
		/// A? ?B ?? DD
		/// </example>
		/// <exception cref="ArgumentException">Thrown if the provided string doesn't contain a valid byte pattern.</exception>
		/// <param name="value">The byte pattern in hex format.</param>
		/// <returns>The corresponding <see cref="BytePattern"/>.</returns>
		public static BytePattern Parse(string value)
		{
			Contract.Requires(!string.IsNullOrEmpty(value));
			Contract.Ensures(Contract.Result<BytePattern>() != null);

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
					throw new ArgumentException($"'{value}' is not a valid byte pattern.");
				}
			}

			return pattern;
		}

		public static BytePattern From(IEnumerable<Tuple<byte, bool>> data)
		{
			var pattern = new BytePattern();

			foreach (var i in data)
			{
				var pb = i.Item2 ? PatternByte.AsWildcard() : PatternByte.AsByte(i.Item1);

				pattern.pattern.Add(pb);
			}

			return pattern;
		}

		/// <summary>
		/// Tests if the provided byte array matches the byte pattern at the provided index.
		/// </summary>
		/// <param name="data">The byte array to be compared.</param>
		/// <param name="index">The index into the byte array.</param>
		/// <returns>True if the pattern matches, false if they are not.</returns>
		public bool Equals(byte[] data, int index)
		{
			Contract.Requires(data != null);

			for (var j = 0; j < pattern.Count; ++j)
			{
				if (!pattern[j].Equals(data[index + j]))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Converts this <see cref="BytePattern"/> to a byte array.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if the pattern contains wildcards.</exception>
		/// <returns>The bytes of the pattern.
		/// </returns>
		public byte[] ToByteArray()
		{
			Contract.Ensures(Contract.Result<byte[]>() != null);

			if (HasWildcards)
			{
				throw new InvalidOperationException();
			}

			return pattern.Select(pb => pb.ByteValue).ToArray();
		}

		/// <summary>
		/// Formats the <see cref="BytePattern"/> in the specified <see cref="PatternFormat"/>.
		/// </summary>
		/// <param name="format">The format of the pattern.</param>
		/// <returns>A tuple containing the format. If <paramref name="format"/> is not <see cref="PatternFormat.PatternAndMask"/> the second item is null.</returns>
		public Tuple<string, string> ToString(PatternFormat format)
		{
			switch (format)
			{
				case PatternFormat.PatternAndMask:
					var sb1 = new StringBuilder();
					var sb2 = new StringBuilder();
					pattern
						.Select(p => p.ToString(PatternFormat.PatternAndMask))
						.ForEach(t =>
						{
							sb1.Append(t.Item1);
							sb2.Append(t.Item2);
						});
					return Tuple.Create(sb1.ToString(), sb2.ToString());
				case PatternFormat.Combined:
					return Tuple.Create<string, string>(string.Join(" ", pattern.Select(p => p.ToString(PatternFormat.Combined).Item1)), null);
				default:
					throw new ArgumentOutOfRangeException(nameof(format), format, null);
			}
		}

		public override string ToString() => ToString(PatternFormat.Combined).Item1;
	}
}
