namespace ReClassNET.AddressParser
{
	public interface ITokenizer
	{
		/// <summary>
		/// The current token. It is set to <see cref="Token.None"/> if no more tokens are avaiable.
		/// </summary>
		Token Token { get; }

		/// <summary>
		/// The current identifier.
		/// </summary>
		string Identifier { get; }

		/// <summary>
		/// The current number.
		/// </summary>
		long Number { get; }

		/// <summary>
		/// Reads the next token.
		/// </summary>
		void ReadNextToken();
	}
}
