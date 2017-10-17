using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Input
{
	public class KeyboardHotkey
	{
		private readonly HashSet<Keys> keys = new HashSet<Keys>();

		public IEnumerable<Keys> Keys => keys;

		public bool IsEmpty => keys.Count == 0;

		public void Clear() => keys.Clear();

		public bool AddKey(Keys key) => keys.Add(key);

		public bool Matches(Keys[] pressedKeys)
		{
			Contract.Requires(pressedKeys != null);

			if (keys.Count == 0 || keys.Count > pressedKeys.Length)
			{
				return false;
			}

			return keys.All(pressedKeys.Contains);
		}

		public KeyboardHotkey Clone()
		{
			var copy = new KeyboardHotkey();
			foreach (var key in Keys) copy.AddKey(key);
			return copy;
		}

		public override string ToString()
		{
			if (keys.Count == 0)
			{
				return string.Empty;
			}
			return keys.Select(k => k.ToString()).Aggregate((s1, s2) => $"{s1} + {s2}");
		}
	}
}
