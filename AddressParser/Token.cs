// Design taken from https://github.com/pieterderycke/Jace

namespace ReClassNET.AddressParser
{
	enum TokenType
	{
		Offset,
		ModuleOffset,
		Operation,
		LeftBracket,
		RightBracket,
		ReadPointer
	}

	class Token
	{
		/// <summary>
		/// The type of the token.
		/// </summary>
		public TokenType TokenType;

		/// <summary>
		/// The value of the token.
		/// </summary>
		public object Value;

		public override string ToString()
		{
			return $"{TokenType} {Value}";
		}
	}
}
