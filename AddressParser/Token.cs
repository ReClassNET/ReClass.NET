// Design taken from https://github.com/pieterderycke/Jace

using System.Diagnostics.Contracts;

namespace ReClassNET.AddressParser
{
	internal enum TokenType
	{
		Offset,
		ModuleOffset,
		Operation,
		LeftBracket,
		RightBracket,
		ReadPointer
	}

	internal class Token
	{
		/// <summary>The type of the token.</summary>
		public TokenType TokenType { get; }

		/// <summary>The value of the token.</summary>
		public object Value { get; }

		public Token(TokenType type, object value)
		{
			Contract.Requires(value != null);

			TokenType = type;
			Value = value;
		}

		public override string ToString()
		{
			return $"{TokenType} {Value}";
		}
	}
}
