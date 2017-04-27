using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	class SettingsCheckBox : CheckBox, ISettingsBindable
	{
		private PropertyInfo property;
		private Settings source;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SettingName
		{
			get => property?.Name;
			set { property = typeof(Settings).GetProperty(value); ReadSetting(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Settings Source
		{
			get => source;
			set { source = value; ReadSetting(); }
		}

		private void ReadSetting()
		{
			if (property != null && source != null)
			{
				var value = property.GetValue(source);
				if (value is bool)
				{
					Checked = (bool)value;
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
