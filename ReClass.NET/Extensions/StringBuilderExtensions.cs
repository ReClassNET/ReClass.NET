using System.Text;

namespace ReClassNET.Extensions
{
	public static class StringBuilderExtensions
	{
		public static StringBuilder Prepend(this StringBuilder sb, char value)
		{
			return sb.Insert(0, value);
		}

		public static StringBuilder Prepend(this StringBuilder sb, string value)
		{
			return sb.Insert(0, value);
		}
	}
}
