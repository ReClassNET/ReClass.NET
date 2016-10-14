// Design taken from https://github.com/pieterderycke/Jace

using System;

namespace ReClassNET.AddressParser
{
	class ParseException : Exception
	{
		public ParseException(string message)
			: base(message)
		{

		}
	}
}
