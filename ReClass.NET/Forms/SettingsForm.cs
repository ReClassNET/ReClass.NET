using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ReClassNET.Extensions;
using ReClassNET.Native;
using ReClassNET.Project;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class SettingsForm : IconForm
	{
		private readonly Settings settings;
		private readonly CppTypeMapping typeMapping;

		public TabControl SettingsTabControl => settingsTabControl;

		public SettingsForm(Settings settings, CppTypeMapping typeMapping)
		{
			Contract.Requires(settings != null);
			Contract.Requires(typeMapping != null);

			this.settings = settings;
			this.typeMapping = typeMapping;

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
			stayOnTopCheckBox.PropertyName = nameof(Settings.StayOnTop);
			stayOnTopCheckBox.CheckedChanged += (sender, e) =>
			{
				GlobalWindowManager.Windows.ForEach(w => w.TopMost = stayOnTopCheckBox.Checked);
			};

			showNodeAddressCheckBox.Source = settings;
			showNodeAddressCheckBox.PropertyName = nameof(Settings.ShowNodeAddress);
			showNodeOffsetCheckBox.Source = settings;
			showNodeOffsetCheckBox.PropertyName = nameof(Settings.ShowNodeOffset);
			showTextCheckBox.Source = settings;
			showTextCheckBox.PropertyName = nameof(Settings.ShowNodeText);
			highlightChangedValuesCheckBox.Source = settings;
			highlightChangedValuesCheckBox.PropertyName = nameof(Settings.HighlightChangedValues);

			showFloatCheckBox.Source = settings;
			showFloatCheckBox.PropertyName = nameof(Settings.ShowCommentFloat);
			showIntegerCheckBox.Source = settings;
			showIntegerCheckBox.PropertyName = nameof(Settings.ShowCommentInteger);
			showPointerCheckBox.Source = settings;
			showPointerCheckBox.PropertyName = nameof(Settings.ShowCommentPointer);
			showRttiCheckBox.Source = settings;
			showRttiCheckBox.PropertyName = nameof(Settings.ShowCommentRtti);
			showSymbolsCheckBox.Source = settings;
			showSymbolsCheckBox.PropertyName = nameof(Settings.ShowCommentSymbol);
			showStringCheckBox.Source = settings;
			showStringCheckBox.PropertyName = nameof(Settings.ShowCommentString);
			showPluginInfoCheckBox.Source = settings;
			showPluginInfoCheckBox.PropertyName = nameof(Settings.ShowCommentPluginInfo);
		}

		private void SetColorBindings()
		{
			backgroundColorBox.Source = settings;
			backgroundColorBox.PropertyName = nameof(Settings.BackgroundColor);

			nodeSelectedColorBox.Source = settings;
			nodeSelectedColorBox.PropertyName = nameof(Settings.SelectedColor);
			nodeHiddenColorBox.Source = settings;
			nodeHiddenColorBox.PropertyName = nameof(Settings.HiddenColor);
			nodeAddressColorBox.Source = settings;
			nodeAddressColorBox.PropertyName = nameof(Settings.AddressColor);
			nodeOffsetColorBox.Source = settings;
			nodeOffsetColorBox.PropertyName = nameof(Settings.OffsetColor);
			nodeHexValueColorBox.Source = settings;
			nodeHexValueColorBox.PropertyName = nameof(Settings.HexColor);
			nodeTypeColorBox.Source = settings;
			nodeTypeColorBox.PropertyName = nameof(Settings.TypeColor);
			nodeNameColorBox.Source = settings;
			nodeNameColorBox.PropertyName = nameof(Settings.NameColor);
			nodeValueColorBox.Source = settings;
			nodeValueColorBox.PropertyName = nameof(Settings.ValueColor);
			nodeIndexColorBox.Source = settings;
			nodeIndexColorBox.PropertyName = nameof(Settings.IndexColor);
			nodeVTableColorBox.Source = settings;
			nodeVTableColorBox.PropertyName = nameof(Settings.VTableColor);
			nodeCommentColorBox.Source = settings;
			nodeCommentColorBox.PropertyName = nameof(Settings.CommentColor);
			nodeTextColorBox.Source = settings;
			nodeTextColorBox.PropertyName = nameof(Settings.TextColor);
			nodePluginColorBox.Source = settings;
			nodePluginColorBox.PropertyName = nameof(Settings.PluginColor);
		}

		private void SetTypedefinitionBindings()
		{
			boolSettingsTextBox.Source = typeMapping;
			boolSettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeBool);
			int8SettingsTextBox.Source = typeMapping;
			int8SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeInt8);
			int16SettingsTextBox.Source = typeMapping;
			int16SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeInt16);
			int32SettingsTextBox.Source = typeMapping;
			int32SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeInt32);
			int64SettingsTextBox.Source = typeMapping;
			int64SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeInt64);
			uint8SettingsTextBox.Source = typeMapping;
			uint8SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeUInt8);
			uint16SettingsTextBox.Source = typeMapping;
			uint16SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeUInt16);
			uint32SettingsTextBox.Source = typeMapping;
			uint32SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeUInt32);
			uint64SettingsTextBox.Source = typeMapping;
			uint64SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeUInt64);
			floatSettingsTextBox.Source = typeMapping;
			floatSettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeFloat);
			doubleSettingsTextBox.Source = typeMapping;
			doubleSettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeDouble);
			vector2SettingsTextBox.Source = typeMapping;
			vector2SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeVector2);
			vector3SettingsTextBox.Source = typeMapping;
			vector3SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeVector3);
			vector4SettingsTextBox.Source = typeMapping;
			vector4SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeVector4);
			matrix3x3SettingsTextBox.Source = typeMapping;
			matrix3x3SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeMatrix3x3);
			matrix3x4SettingsTextBox.Source = typeMapping;
			matrix3x4SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeMatrix3x4);
			matrix4x4SettingsTextBox.Source = typeMapping;
			matrix4x4SettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeMatrix4x4);
			utf8TextSettingsTextBox.Source = typeMapping;
			utf8TextSettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeUtf8Text);
			utf16TextSettingsTextBox.Source = typeMapping;
			utf16TextSettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeUtf16Text);
			functionPtrSettingsTextBox.Source = typeMapping;
			functionPtrSettingsTextBox.PropertyName = nameof(CppTypeMapping.TypeFunctionPtr);
		}
	}
}
