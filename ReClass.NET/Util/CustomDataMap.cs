using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace ReClassNET.Util
{
	/// <summary>
	/// A class which stores custom data items from plugins.
	/// The key to an item should consist of "a-zA-z0-9.,;_-+".
	/// The naming convention for keys is "PluginName.[Group.]Item".
	/// </summary>
	public class CustomDataMap
	{
		private readonly Dictionary<string, string> data = new Dictionary<string, string>();

		internal XElement Serialize(string name)
		{
			return XElementSerializer.ToXml(name, data);
		}

		internal void Deserialize(XElement element)
		{
			foreach (var kv in XElementSerializer.ToDictionary(element))
			{
				data[kv.Key] = kv.Value;
			}
		}

		/// <summary>
		/// Sets a configuration item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="value">
		/// The value of the item.
		/// If the value is null the item gets removed.
		/// </param>
		public void SetString(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			if (key.Length == 0)
			{
				throw new ArgumentException();
			}

			if (value == null)
			{
				data.Remove(key);
			}
			else
			{
				data[key] = value;
			}
		}

		/// <summary>
		/// Sets a configuration item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="value">The value of the item.</param>
		public void SetBool(string key, bool value)
		{
			SetString(key, Convert.ToString(value));
		}

		/// <summary>
		/// Sets a configuration item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="value">The value of the item.</param>
		public void SetLong(string key, long value)
		{
			SetString(key, value.ToString(NumberFormatInfo.InvariantInfo));
		}

		/// <summary>
		/// Sets a configuration item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="value">The value of the item.</param>
		public void SetULong(string key, ulong value)
		{
			SetString(key, value.ToString(NumberFormatInfo.InvariantInfo));
		}

		public void SetXElement(string key, XElement value)
		{
			SetString(key, value.ToString());
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <returns>The value of the config item or null if the key does not exists.</returns>
		public string GetString(string key)
		{
			return GetString(key, null);
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="def">The default value if the key does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the key does not exists.</returns>
		public string GetString(string key, string def)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			if (key.Length == 0)
			{
				throw new ArgumentException();
			}

			if (data.TryGetValue(key, out var value))
			{
				return value;
			}

			return def;
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="def">The default value if the key does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the key does not exists.</returns>
		public bool GetBool(string key, bool def)
		{
			var value = GetString(key, null);
			if (string.IsNullOrEmpty(value))
			{
				return def;
			}

			return Convert.ToBoolean(value);
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="def">The default value if the key does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the key does not exists.</returns>
		public long GetLong(string key, long def)
		{
			var str = GetString(key, null);
			if (string.IsNullOrEmpty(str))
			{
				return def;
			}

			if (long.TryParse(str, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var value))
			{
				return value;
			}

			return def;
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="def">The default value if the key does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the key does not exists.</returns>
		public ulong GetULong(string key, ulong def)
		{
			var str = GetString(key, null);
			if (string.IsNullOrEmpty(str))
			{
				return def;
			}

			if (ulong.TryParse(str, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var value))
			{
				return value;
			}

			return def;
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="def">The default value if the key does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the key does not exists.</returns>
		public XElement GetXElement(string key, XElement def)
		{
			var str = GetString(key, null);
			if (string.IsNullOrEmpty(str))
			{
				return def;
			}

			return XElement.Parse(str);
		}
	}
}
