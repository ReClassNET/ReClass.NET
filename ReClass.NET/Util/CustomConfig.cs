using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace ReClassNET.Util
{
	/// <summary>
	/// A class which stores custom config items from plugins.
	/// The id of an item should consist of "a-zA-z0-9.,;_-+".
	/// The naming convention is "Plugin.[ConfigGroup.]ConfigItem".
	/// </summary>
	public class CustomConfig
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
		/// <param name="id">The id of the item.</param>
		/// <param name="value">
		/// The value of the item.
		/// If the value is null the item gets removed.
		/// </param>
		public void SetString(string id, string value)
		{
			if (id == null)
			{
				throw new ArgumentNullException(nameof(id));
			}
			if (id.Length == 0)
			{
				throw new ArgumentException();
			}

			if (value == null)
			{
				data.Remove(id);
			}
			else
			{
				data[id] = value;
			}
		}

		/// <summary>
		/// Sets a configuration item.
		/// </summary>
		/// <param name="id">The id of the item.</param>
		/// <param name="value">The value of the item.</param>
		public void SetBool(string id, bool value)
		{
			SetString(id, Convert.ToString(value));
		}

		/// <summary>
		/// Sets a configuration item.
		/// </summary>
		/// <param name="id">The id of the item.</param>
		/// <param name="value">The value of the item.</param>
		public void SetLong(string id, long value)
		{
			SetString(id, value.ToString(NumberFormatInfo.InvariantInfo));
		}

		/// <summary>
		/// Sets a configuration item.
		/// </summary>
		/// <param name="id">The id of the item.</param>
		/// <param name="value">The value of the item.</param>
		public void SetULong(string id, ulong value)
		{
			SetString(id, value.ToString(NumberFormatInfo.InvariantInfo));
		}

		public void SetXElement(string id, XElement value)
		{
			SetString(id, value.ToString());
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="id">The id of the item.</param>
		/// <returns>The value of the config item or null if the id does not exists.</returns>
		public string GetString(string id)
		{
			return GetString(id, null);
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="id">The id of the item.</param>
		/// <param name="def">The default value if the id does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the id does not exists.</returns>
		public string GetString(string id, string def)
		{
			if (id == null)
			{
				throw new ArgumentNullException(nameof(id));
			}
			if (id.Length == 0)
			{
				throw new ArgumentException();
			}

			if (data.TryGetValue(id, out var value))
			{
				return value;
			}

			return def;
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="id">The id of the item.</param>
		/// <param name="def">The default value if the id does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the id does not exists.</returns>
		public bool GetBool(string id, bool def)
		{
			var value = GetString(id, null);
			if (string.IsNullOrEmpty(value))
			{
				return def;
			}

			return Convert.ToBoolean(value);
		}

		/// <summary>
		/// Gets the value of the config item.
		/// </summary>
		/// <param name="id">The id of the item.</param>
		/// <param name="def">The default value if the id does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the id does not exists.</returns>
		public long GetLong(string id, long def)
		{
			var str = GetString(id, null);
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
		/// <param name="id">The id of the item.</param>
		/// <param name="def">The default value if the id does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the id does not exists.</returns>
		public ulong GetULong(string id, ulong def)
		{
			var str = GetString(id, null);
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
		/// <param name="id">The id of the item.</param>
		/// <param name="def">The default value if the id does not exists.</param>
		/// <returns>The value of the config item or <paramref name="def"/> if the id does not exists.</returns>
		public XElement GetXElement(string id, XElement def)
		{
			var str = GetString(id, null);
			if (string.IsNullOrEmpty(str))
			{
				return def;
			}

			return XElement.Parse(str);
		}
	}
}
