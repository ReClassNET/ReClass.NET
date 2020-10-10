using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace ReClassNET.UI
{
	public class EnumDescriptionDisplay<TEnum> where TEnum : struct
	{
		public TEnum Value { get; internal set; }
		public string Description { get; internal set; }

		public static List<EnumDescriptionDisplay<TEnum>> Create()
		{
			Contract.Ensures(Contract.Result<List<EnumDescriptionDisplay<TEnum>>>() != null);

			return CreateExact(Enum.GetValues(typeof(TEnum)).Cast<TEnum>());
		}

		public static List<EnumDescriptionDisplay<TEnum>> CreateExact(IEnumerable<TEnum> include)
		{
			Contract.Requires(include != null);
			Contract.Ensures(Contract.Result<List<EnumDescriptionDisplay<TEnum>>>() != null);

			return include
				.Select(value => new EnumDescriptionDisplay<TEnum>
				{
					Description = GetDescription(value),
					Value = value
				})
				.OrderBy(item => item.Value)
				.ToList();
		}

		public static List<EnumDescriptionDisplay<TEnum>> CreateExclude(IEnumerable<TEnum> exclude)
		{
			Contract.Requires(exclude != null);
			Contract.Ensures(Contract.Result<List<EnumDescriptionDisplay<TEnum>>>() != null);

			return Enum.GetValues(typeof(TEnum))
				.Cast<TEnum>()
				.Except(exclude)
				.Select(value => new EnumDescriptionDisplay<TEnum>
				{
					Description = GetDescription(value),
					Value = value
				})
				.OrderBy(item => item.Value)
				.ToList();
		}

		private static string GetDescription(TEnum value)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return value.GetType().GetField(value.ToString()).GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
		}
	}
}
