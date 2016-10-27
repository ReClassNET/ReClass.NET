using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class SettingsForm : IconForm
	{
		private readonly Settings settings;

		public SettingsForm(Settings settings)
		{
			Contract.Requires(settings != null);

			this.settings = settings;

			InitializeComponent();

			var imageList = new ImageList();
			imageList.Images.Add(Properties.Resources.B16x16_Gear);
			imageList.Images.Add(Properties.Resources.B16x16_Color_Wheel);
			imageList.Images.Add(Properties.Resources.B16x16_Settings_Edit);

			settingsTabControl.ImageList = imageList;
			generalSettingsTabPage.ImageIndex = 0;
			colorsSettingTabPage.ImageIndex = 1;
			typeDefinitionsSettingsTabPage.ImageIndex = 2;

			backgroundColorBox.Color = System.Drawing.Color.Red;

			SetBindings();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			GlobalWindowManager.RemoveWindow(this);
		}

		private void SetBindings()
		{
			SetGeneralBindings();
			SetColorBindings();
			SetTypedefinitionBindings();
		}

		private void SetGeneralBindings()
		{
			stayOnTopCheckBox.Source = settings;
			stayOnTopCheckBox.SettingName = nameof(Settings.StayOnTop);

			showNodeAddressCheckBox.Source = settings;
			showNodeAddressCheckBox.SettingName = nameof(Settings.ShowNodeAddress);
			showNodeOffsetCheckBox.Source = settings;
			showNodeOffsetCheckBox.SettingName = nameof(Settings.ShowNodeOffset);
			showTextCheckBox.Source = settings;
			showTextCheckBox.SettingName = nameof(Settings.ShowNodeText);
			highlightChangedValuesCheckBox.Source = settings;
			highlightChangedValuesCheckBox.SettingName = nameof(Settings.HighlightChangedValues);

			showFloatCheckBox.Source = settings;
			showFloatCheckBox.SettingName = nameof(Settings.ShowCommentFloat);
			showIntegerCheckBox.Source = settings;
			showIntegerCheckBox.SettingName = nameof(Settings.ShowCommentInteger);
			showPointerCheckBox.Source = settings;
			showPointerCheckBox.SettingName = nameof(Settings.ShowCommentPointer);
			showRttiCheckBox.Source = settings;
			showRttiCheckBox.SettingName = nameof(Settings.ShowCommentRtti);
			showSymbolsCheckBox.Source = settings;
			showSymbolsCheckBox.SettingName = nameof(Settings.ShowCommentSymbol);
			showStringCheckBox.Source = settings;
			showStringCheckBox.SettingName = nameof(Settings.ShowCommentString);
			showPluginInfoCheckBox.Source = settings;
			showPluginInfoCheckBox.SettingName = nameof(Settings.ShowCommentPluginInfo);
		}

		private void SetColorBindings()
		{
			backgroundColorBox.Source = settings;
			backgroundColorBox.SettingName = nameof(Settings.BackgroundColor);

			nodeSelectedColorBox.Source = settings;
			nodeSelectedColorBox.SettingName = nameof(Settings.SelectedColor);
			nodeHiddenColorBox.Source = settings;
			nodeHiddenColorBox.SettingName = nameof(Settings.HiddenColor);
			nodeAddressColorBox.Source = settings;
			nodeAddressColorBox.SettingName = nameof(Settings.AddressColor);
			nodeOffsetColorBox.Source = settings;
			nodeOffsetColorBox.SettingName = nameof(Settings.OffsetColor);
			nodeHexValueColorBox.Source = settings;
			nodeHexValueColorBox.SettingName = nameof(Settings.HexColor);
			nodeTypeColorBox.Source = settings;
			nodeTypeColorBox.SettingName = nameof(Settings.TypeColor);
			nodeNameColorBox.Source = settings;
			nodeNameColorBox.SettingName = nameof(Settings.NameColor);
			nodeValueColorBox.Source = settings;
			nodeValueColorBox.SettingName = nameof(Settings.ValueColor);
			nodeIndexColorBox.Source = settings;
			nodeIndexColorBox.SettingName = nameof(Settings.IndexColor);
			nodeVTableColorBox.Source = settings;
			nodeVTableColorBox.SettingName = nameof(Settings.VTableColor);
			nodeCommentColorBox.Source = settings;
			nodeCommentColorBox.SettingName = nameof(Settings.CommentColor);
			nodeTextColorBox.Source = settings;
			nodeTextColorBox.SettingName = nameof(Settings.TextColor);
			nodePluginColorBox.Source = settings;
			nodePluginColorBox.SettingName = nameof(Settings.PluginColor);
		}

		private void SetTypedefinitionBindings()
		{
			paddingTextBox.Source = settings;
			paddingTextBox.SettingName = nameof(Settings.TypePadding);
			int8TextBox.Source = settings;
			int8TextBox.SettingName = nameof(Settings.TypeInt8);
			int16TextBox.Source = settings;
			int16TextBox.SettingName = nameof(Settings.TypeInt16);
			int32TextBox.Source = settings;
			int32TextBox.SettingName = nameof(Settings.TypeInt32);
			int64TextBox.Source = settings;
			int64TextBox.SettingName = nameof(Settings.TypeInt64);
			uint8TextBox.Source = settings;
			uint8TextBox.SettingName = nameof(Settings.TypeUInt8);
			uint16TextBox.Source = settings;
			uint16TextBox.SettingName = nameof(Settings.TypeUInt16);
			uint32TextBox.Source = settings;
			uint32TextBox.SettingName = nameof(Settings.TypeUInt32);
			uint64TextBox.Source = settings;
			uint64TextBox.SettingName = nameof(Settings.TypeUInt64);
			floatTextBox.Source = settings;
			floatTextBox.SettingName = nameof(Settings.TypeFloat);
			doubleTextBox.Source = settings;
			doubleTextBox.SettingName = nameof(Settings.TypeDouble);
			vector2TextBox.Source = settings;
			vector2TextBox.SettingName = nameof(Settings.TypeVector2);
			vector3TextBox.Source = settings;
			vector3TextBox.SettingName = nameof(Settings.TypeVector3);
			vector4TextBox.Source = settings;
			vector4TextBox.SettingName = nameof(Settings.TypeVector4);
			matrix3x3TextBox.Source = settings;
			matrix3x3TextBox.SettingName = nameof(Settings.TypeMatrix3x3);
			matrix3x4TextBox.Source = settings;
			matrix3x4TextBox.SettingName = nameof(Settings.TypeMatrix3x4);
			matrix4x4TextBox.Source = settings;
			matrix4x4TextBox.SettingName = nameof(Settings.TypeMatrix4x4);
			utf8TextTextBox.Source = settings;
			utf8TextTextBox.SettingName = nameof(Settings.TypeUTF8Text);
			utf8TextPtrTextBox.Source = settings;
			utf8TextPtrTextBox.SettingName = nameof(Settings.TypeUTF8TextPtr);
			utf16TextTextBox.Source = settings;
			utf16TextTextBox.SettingName = nameof(Settings.TypeUTF16Text);
			utf16TextPtrTextBox.Source = settings;
			utf16TextPtrTextBox.SettingName = nameof(Settings.TypeUTF16TextPtr);
			functionPtrTextBox.Source = settings;
			functionPtrTextBox.SettingName = nameof(Settings.TypeFunctionPtr);
		}
	}
}
