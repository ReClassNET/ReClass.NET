using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ReClassNET.Extensions
{
	public static class DictionaryExtension
	{
		public static void RemoveWhere<TKey, TValue>(this IDictionary<TKey, TValue> source, Func<KeyValuePair<TKey, TValue>, bool> selector)
		{
			Contract.Requires(source != null);
			Contract.Requires(selector != null);

			foreach (var kv in source.Where(selector).ToList())
			{
				source.Remove(kv.Key);
			}
		}
	}
}
