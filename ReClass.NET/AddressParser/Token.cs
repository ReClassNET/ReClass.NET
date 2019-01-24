using System.Diagnostics.Contracts;

namespace ReClassNET.AddressParser
{
	public enum TokenType
	{
		Offset,
		ModuleOffset,
		Operation,
		LeftBracket,
		RightBracket,
		ReadPointer
	}

	public class Token
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
