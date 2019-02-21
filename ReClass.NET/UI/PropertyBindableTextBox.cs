using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	public class PropertyBindableTextBox : TextBox, IPropertyBindable
	{
		private PropertyInfo property;
		private object source;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string PropertyName
		{
			get => property?.Name;
			set { property = source?.GetType().GetProperty(value); ReadSetting(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Source
		{
			get => source;
			set { source = value; ReadSetting(); }
		}

		private void ReadSetting()
		{
			if (property != null && source != null)
			{
				var value = property.GetValue(source);
				if (value is string s)
				{
					Text = s;
				}
			}
		}

		private void WriteSetting()
		{
			if (property != null && source != null)
			{
				property.SetValue(source, Text);
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			WriteSetting();
		}
	}
}
