using System.Diagnostics.Contracts;
using System.Globalization;

namespace ReClassNET.Util
{
	public class NumberFormat
	{
		public static NumberFormatInfo GuessNumberFormat(string input)
		{
			Contract.Requires(input != null);
			Contract.Ensures(Contract.Result<NumberFormatInfo>() != null);

			if (input.Contains(",") && !input.Contains("."))
			{
				return new NumberFormatInfo
				{
					NumberDecimalSeparator = ",",
					NumberGroupSeparator = "."
				};
			}
			return new NumberFormatInfo
			{
				NumberDecimalSeparator = ".",
				NumberGroupSeparator = ","
			};
		}
	}
}
