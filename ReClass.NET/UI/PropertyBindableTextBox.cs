using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	public class PropertyBindableTextBox : TextBox, IPropertyBindable
	{
		private string propertyName;
		private object source;
		private PropertyInfo property;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string PropertyName
		{
			get => propertyName;
			set
			{
				propertyName = value;
				property = null;

				ReadSetting();
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Source
		{
			get => source;
			set
			{
				source = value;
				property = null;

				ReadSetting();
			}
		}

		private void TryGetPropertyInfo()
		{
			if (property == null && source != null && !string.IsNullOrEmpty(propertyName))
			{
				property = source?.GetType().GetProperty(propertyName);
			}
		}

		private void ReadSetting()
		{
			TryGetPropertyInfo();

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
			TryGetPropertyInfo();

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
