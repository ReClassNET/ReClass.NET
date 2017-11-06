using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ReClassNET.Native;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class SettingsForm : IconForm
	{
		private readonly Settings settings;

		public TabControl SettingsTabControl => settingsTabControl;

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

			if (NativeMethods.IsUnix())
			{
				fileAssociationGroupBox.Enabled = false;
			}
			else
			{
				NativeMethodsWindows.SetButtonShield(createAssociationButton, true);
				NativeMethodsWindows.SetButtonShield(removeAssociationButton, true);
			}
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

		private void createAssociationButton_Click(object sender, EventArgs e)
		{
			WinUtil.RunElevated(PathUtil.LauncherExecutablePath, $"-{Constants.CommandLineOptions.FileExtRegister}");
		}

		private void removeAssociationButton_Click(object sender, EventArgs e)
		{
			WinUtil.RunElevated(PathUtil.LauncherExecutablePath, $"-{Constants.CommandLineOptions.FileExtUnregister}");
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
			stayOnTopCheckBox.CheckedChanged += (sender, e) =>
			{
				GlobalWindowManager.Windows.ForEach(w => w.TopMost = stayOnTopCheckBox.Checked);
			};

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
			paddingSettingsTextBox.Source = settings;
			paddingSettingsTextBox.SettingName = nameof(Settings.TypePadding);
			boolSettingsTextBox.Source = settings;
			boolSettingsTextBox.SettingName = nameof(Settings.TypeBool);
			int8SettingsTextBox.Source = settings;
			int8SettingsTextBox.SettingName = nameof(Settings.TypeInt8);
			int16SettingsTextBox.Source = settings;
			int16SettingsTextBox.SettingName = nameof(Settings.TypeInt16);
			int32SettingsTextBox.Source = settings;
			int32SettingsTextBox.SettingName = nameof(Settings.TypeInt32);
			int64SettingsTextBox.Source = settings;
			int64SettingsTextBox.SettingName = nameof(Settings.TypeInt64);
			uint8SettingsTextBox.Source = settings;
			uint8SettingsTextBox.SettingName = nameof(Settings.TypeUInt8);
			uint16SettingsTextBox.Source = settings;
			uint16SettingsTextBox.SettingName = nameof(Settings.TypeUInt16);
			uint32SettingsTextBox.Source = settings;
			uint32SettingsTextBox.SettingName = nameof(Settings.TypeUInt32);
			uint64SettingsTextBox.Source = settings;
			uint64SettingsTextBox.SettingName = nameof(Settings.TypeUInt64);
			floatSettingsTextBox.Source = settings;
			floatSettingsTextBox.SettingName = nameof(Settings.TypeFloat);
			doubleSettingsTextBox.Source = settings;
			doubleSettingsTextBox.SettingName = nameof(Settings.TypeDouble);
			vector2SettingsTextBox.Source = settings;
			vector2SettingsTextBox.SettingName = nameof(Settings.TypeVector2);
			vector3SettingsTextBox.Source = settings;
			vector3SettingsTextBox.SettingName = nameof(Settings.TypeVector3);
			vector4SettingsTextBox.Source = settings;
			vector4SettingsTextBox.SettingName = nameof(Settings.TypeVector4);
			matrix3x3SettingsTextBox.Source = settings;
			matrix3x3SettingsTextBox.SettingName = nameof(Settings.TypeMatrix3x3);
			matrix3x4SettingsTextBox.Source = settings;
			matrix3x4SettingsTextBox.SettingName = nameof(Settings.TypeMatrix3x4);
			matrix4x4SettingsTextBox.Source = settings;
			matrix4x4SettingsTextBox.SettingName = nameof(Settings.TypeMatrix4x4);
			utf8TextSettingsTextBox.Source = settings;
			utf8TextSettingsTextBox.SettingName = nameof(Settings.TypeUTF8Text);
			utf8TextPtrSettingsTextBox.Source = settings;
			utf8TextPtrSettingsTextBox.SettingName = nameof(Settings.TypeUTF8TextPtr);
			utf16TextSettingsTextBox.Source = settings;
			utf16TextSettingsTextBox.SettingName = nameof(Settings.TypeUTF16Text);
			utf16TextPtrSettingsTextBox.Source = settings;
			utf16TextPtrSettingsTextBox.SettingName = nameof(Settings.TypeUTF16TextPtr);
			functionPtrSettingsTextBox.Source = settings;
			functionPtrSettingsTextBox.SettingName = nameof(Settings.TypeFunctionPtr);
		}
	}
}
