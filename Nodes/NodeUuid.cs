using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	public sealed class NodeUuid : IComparable<NodeUuid>, IEquatable<NodeUuid>
	{
		/// <summary>Size in bytes of a UUID.</summary>
		public const int UuidSize = 16;

		/// <summary>Zero UUID (all bytes are zero).</summary>
		public static readonly NodeUuid Zero = new NodeUuid(false);

		/// <summary>The maximum reserved UUID value.</summary>
		private static readonly NodeUuid MaxReserved = new NodeUuid(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF, 0xFF });

		/// <summary>Checks if the given UUID is reserved.</summary>
		/// <param name="uuid">The uuid.</param>
		/// <returns>True if reserved, false if not.</returns>
		public static bool IsReserved(NodeUuid uuid)
		{
			Contract.Requires(uuid != null);

			return uuid.CompareTo(MaxReserved) <= 0;
		}

		private byte[] uuidBytes;

		/// <summary>Get the 16 UUID bytes.</summary>
		public byte[] UuidBytes => uuidBytes;

		/// <summary>Construct a new UUID object.</summary>
		/// <param name="createNew">If this parameter is <c>true</c>, a new UUID is generated.
		/// If it is <c>false</c>, the UUID is initialized to zero.</param>
		public NodeUuid(bool createNew)
		{
			Contract.Ensures(uuidBytes != null);

			if (createNew)
			{
				CreateNew();
			}
			else
			{
				SetZero();
			}
		}

		/// <summary>Construct a new UUID object.</summary>
		/// <param name="valueBytes">Initial value of the <see cref="NodeUuid"/> object.</param>
		public NodeUuid(byte[] valueBytes)
		{
			Contract.Requires(uuidBytes == null);
			Contract.Requires(valueBytes != null);
			Contract.Requires(valueBytes.Length == UuidSize);
			Contract.Ensures(uuidBytes != null);

			SetValue(valueBytes);
		}

		public static NodeUuid FromBase64String(string base64, bool createNew)
		{
			try
			{
				if (base64 != null)
				{
					var bytes = Convert.FromBase64String(base64);

					if (bytes.Length == UuidSize)
					{
						return new NodeUuid(bytes);
					}
				}
			}
			catch (ArgumentNullException)
			{

			}

			return new NodeUuid(createNew);
		}

		/// <summary>Create a new, random UUID.</summary>
		/// <returns>Returns <c>true</c> if a random UUID has been generated, otherwise it returns <c>false</c>.</returns>
		private void CreateNew()
		{
			Contract.Requires(uuidBytes == null);
			Contract.Ensures(uuidBytes != null);

			while (true)
			{
				uuidBytes = Guid.NewGuid().ToByteArray();

				if (uuidBytes == null || uuidBytes.Length != UuidSize)
				{
					throw new InvalidOperationException();
				}

				// Do not generate reserved UUIDs.
				if (!IsReserved(this))
				{
					break;
				}
			}
		}

		/// <summary>Sets the UUID to the given value.</summary>
		/// <param name="uuidBytes">Initial value of the <see cref="NodeUuid"/> object.</param>
		private void SetValue(byte[] valueBytes)
		{
			Contract.Requires(uuidBytes == null);
			Contract.Requires(valueBytes != null);
			Contract.Requires(valueBytes.Length == UuidSize);
			Contract.Ensures(uuidBytes != null);

			uuidBytes = new byte[UuidSize];

			Array.Copy(valueBytes, uuidBytes, UuidSize);
		}

		/// <summary>Sets the UUID to zero.</summary>
		private void SetZero()
		{
			Contract.Requires(uuidBytes == null);
			Contract.Ensures(uuidBytes != null);

			uuidBytes = new byte[UuidSize];
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as NodeUuid);
		}

		public bool Equals(NodeUuid other)
		{
			if (other == null)
			{
				return false;
			}

			for (int i = 0; i < UuidSize; ++i)
			{
				if (uuidBytes[i] != other.uuidBytes[i])
				{
					return false;
				}
			}

			return true;
		}

		private int hash = 0;
		public override int GetHashCode()
		{
			if (hash == 0)
			{
				unchecked
				{
					hash = 17;
					foreach (var b in uuidBytes)
					{
						hash = hash * 31 + b.GetHashCode();
					}
				}
			}
			return hash;
		}

		public int CompareTo(NodeUuid other)
		{
			Contract.Requires(other != null);

			for (int i = 0; i < UuidSize; ++i)
			{
				if (uuidBytes[i] < other.uuidBytes[i])
				{
					return -1;
				}
				if (uuidBytes[i] > other.uuidBytes[i])
				{
					return 1;
				}
			}

			return 0;
		}

		public string ToBase64String()
		{
			return Convert.ToBase64String(uuidBytes);
		}

		/// <summary>Convert the UUID to its string representation.</summary>
		/// <returns>String containing the UUID value.</returns>
		public string ToHexString()
		{
			return BitConverter.ToString(uuidBytes).Replace("-", string.Empty);
		}

		public override string ToString()
		{
			return ToHexString();
		}
	}
}
