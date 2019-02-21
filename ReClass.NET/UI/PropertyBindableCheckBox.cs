using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	public class PropertyBindableCheckBox : CheckBox, IPropertyBindable
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
				if (value is bool b)
				{
					Checked = b;
				}
			}
		}

		private void WriteSetting()
		{
			if (property != null && source != null)
			{
				property.SetValue(source, Checked);
			}
		}

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);

			WriteSetting();
		}
	}
}
