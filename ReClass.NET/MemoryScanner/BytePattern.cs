using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using ReClassNET.Extensions;

namespace ReClassNET.MemoryScanner
{
	public enum PatternMaskFormat
	{
		/// <summary>
		/// Example: AA BB ?? D? ?E FF
		/// </summary>
		Combined,
		/// <summary>
		/// Example: \xAA\xBB\x00\x00\x00\xFF xx???x
		/// </summary>
		Separated
	}

	public class BytePattern
	{
		private interface IPatternByte
		{
			/// <summary>
			/// Gets the byte value of the pattern byte if possible.
			/// </summary>
			/// <returns></returns>
			byte ToByte();

			/// <summary>
			/// Compares the pattern byte with the given byte.
			/// </summary>
			/// <param name="b"></param>
			/// <returns></returns>
			bool Equals(byte b);

			/// <summary>
			/// Formats the pattern byte as string.
			/// </summary>
			/// <param name="format"></param>
			/// <returns></returns>
			Tuple<string, string> ToString(PatternMaskFormat format);
		}

		private class PatternByte : IPatternByte
		{
			private struct Nibble
			{
				public int Value;
				public bool IsWildcard;
			}

			private Nibble nibble1;
			private Nibble nibble2;

			public bool HasWildcard => nibble1.IsWildcard || nibble2.IsWildcard;

			public byte ToByte() => !HasWildcard ? (byte)((nibble1.Value << 4) + nibble2.Value) : throw new InvalidOperationException();

			public static PatternByte NewWildcardByte()
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
				if (nibble1.IsWildcard || ((b >> 4) & 0xF) == nibble1.Value)
				{
					if (nibble2.IsWildcard || (b & 0xF) == nibble2.Value)
					{
						return true;
					}
				}

				return false;
			}

			public Tuple<string, string> ToString(PatternMaskFormat format)
			{
				switch (format)
				{
					case PatternMaskFormat.Separated:
						return HasWildcard ? Tuple.Create("\\x00", "?") : Tuple.Create($"\\x{ToByte():X02}", "x");
					case PatternMaskFormat.Combined:
						var sb = new StringBuilder();
						if (nibble1.IsWildcard) sb.Append('?');
						else sb.AppendFormat("{0:X}", nibble1.Value);
						if (nibble2.IsWildcard) sb.Append('?');
						else sb.AppendFormat("{0:X}", nibble2.Value);
						return Tuple.Create(sb.ToString(), (string)null);
					default:
						throw new ArgumentOutOfRangeException(nameof(format), format, null);
				}
			}

			public override string ToString() => ToString(PatternMaskFormat.Combined).Item1;
		}

		private class SimplePatternByte : IPatternByte
		{
			private readonly byte value;

			public SimplePatternByte(byte value)
			{
				this.value = value;
			}

			public byte ToByte() => value;

			public bool Equals(byte b) => value == b;

			public Tuple<string, string> ToString(PatternMaskFormat format)
			{
				switch (format)
				{
					case PatternMaskFormat.Separated:
						return Tuple.Create($"\\x{ToByte():X02}", "x");
					case PatternMaskFormat.Combined:
						return Tuple.Create($"{ToByte():X02}", (string)null);
					default:
						throw new ArgumentOutOfRangeException(nameof(format), format, null);
				}
			}
		}

		private readonly List<IPatternByte> pattern = new List<IPatternByte>();

		/// <summary>
		/// Gets the length of the pattern in byte.
		/// </summary>
		public int Length => pattern.Count;

		/// <summary>
		/// Gets if the pattern contains wildcards.
		/// </summary>
		public bool HasWildcards => pattern.Any(pb => pb is PatternByte pb2 && pb2.HasWildcard);

		private BytePattern()
		{

		}

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
				while (true)
				{
					var pb = new PatternByte();
					if (pb.TryRead(sr))
					{
						if (!pb.HasWildcard)
						{
							pattern.pattern.Add(new SimplePatternByte(pb.ToByte()));
						}
						else
						{
							pattern.pattern.Add(pb);
						}
					}
					else
					{
						break;
					}
				}

				// Check if we are not at the end of the stream
				if (sr.Peek() != -1)
				{
					throw new ArgumentException($"'{value}' is not a valid byte pattern.");
				}
			}

			return pattern;
		}

		/// <summary>
		/// Creates a byte pattern from the provided bytes.
		/// </summary>
		/// <param name="data">The bytes to match.</param>
		/// <returns></returns>
		public static BytePattern From(IEnumerable<byte> data)
		{
			var pattern = new BytePattern();
			pattern.pattern.AddRange(data.Select(b => new SimplePatternByte(b)));
			return pattern;
		}

		/// <summary>
		/// Creates a byte pattern with wildcard support from the provided bytes. The boolean tuple item signals a wildcard.
		/// </summary>
		/// <param name="data">The byte data or the wildcard flag.</param>
		/// <returns></returns>
		public static BytePattern From(IEnumerable<Tuple<byte, bool>> data)
		{
			var pattern = new BytePattern();

			foreach (var (value, isWildcard) in data)
			{
				var pb = isWildcard ? (IPatternByte)PatternByte.NewWildcardByte() : new SimplePatternByte(value);

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

			return pattern.Select(pb => pb.ToByte()).ToArray();
		}

		/// <summary>
		/// Formats the <see cref="BytePattern"/> in the specified <see cref="PatternMaskFormat"/>.
		/// </summary>
		/// <param name="format">The format of the pattern.</param>
		/// <returns>A tuple containing the format. If <paramref name="format"/> is not <see cref="PatternMaskFormat.Separated"/> the second item is null.</returns>
		public Tuple<string, string> ToString(PatternMaskFormat format)
		{
			switch (format)
			{
				case PatternMaskFormat.Separated:
					var sb1 = new StringBuilder();
					var sb2 = new StringBuilder();
					pattern
						.Select(p => p.ToString(PatternMaskFormat.Separated))
						.ForEach(t =>
						{
							sb1.Append(t.Item1);
							sb2.Append(t.Item2);
						});
					return Tuple.Create(sb1.ToString(), sb2.ToString());
				case PatternMaskFormat.Combined:
					return Tuple.Create<string, string>(string.Join(" ", pattern.Select(p => p.ToString(PatternMaskFormat.Combined).Item1)), null);
				default:
					throw new ArgumentOutOfRangeException(nameof(format), format, null);
			}
		}

		public override string ToString() => ToString(PatternMaskFormat.Combined).Item1;
	}
}
