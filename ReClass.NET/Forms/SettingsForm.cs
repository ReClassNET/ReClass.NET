using System;
using System.Diagnostics;
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

			SetGeneralBindings();
			SetColorBindings();
			SetTypedefinitionBindings();

			if (NativeMethods.IsUnix())
			{
				fileAssociationGroupBox.Enabled = false;
				runAsAdminCheckBox.Enabled = false;
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

		private static void SetBinding(IBindableComponent control, string propertyName, object dataSource, string dataMember)
		{
			Contract.Requires(control != null);
			Contract.Requires(propertyName != null);
			Contract.Requires(dataSource != null);
			Contract.Requires(dataMember != null);

			control.DataBindings.Add(propertyName, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void SetGeneralBindings()
		{
			SetBinding(stayOnTopCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.StayOnTop));
			stayOnTopCheckBox.CheckedChanged += (sender, e) =>
			{
				GlobalWindowManager.Windows.ForEach(w => w.TopMost = stayOnTopCheckBox.Checked);
			};

			SetBinding(showNodeAddressCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowNodeAddress));
			SetBinding(showNodeOffsetCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowNodeOffset));
			SetBinding(showTextCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowNodeText));
			SetBinding(highlightChangedValuesCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.HighlightChangedValues));

			SetBinding(showFloatCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentFloat));
			SetBinding(showIntegerCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentInteger));
			SetBinding(showPointerCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentPointer));
			SetBinding(showRttiCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentRtti));
			SetBinding(showSymbolsCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentSymbol));
			SetBinding(showStringCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentString));
			SetBinding(showPluginInfoCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentPluginInfo));
			SetBinding(runAsAdminCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.RunAsAdmin));
			SetBinding(randomizeWindowTitleCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.RandomizeWindowTitle));
		}

		private void SetColorBindings()
		{
			SetBinding(backgroundColorBox, nameof(ColorBox.Color), settings, nameof(Settings.BackgroundColor));

			SetBinding(nodeSelectedColorBox, nameof(ColorBox.Color), settings, nameof(Settings.SelectedColor));
			SetBinding(nodeHiddenColorBox, nameof(ColorBox.Color), settings, nameof(Settings.HiddenColor));
			SetBinding(nodeAddressColorBox, nameof(ColorBox.Color), settings, nameof(Settings.AddressColor));
			SetBinding(nodeOffsetColorBox, nameof(ColorBox.Color), settings, nameof(Settings.OffsetColor));
			SetBinding(nodeHexValueColorBox, nameof(ColorBox.Color), settings, nameof(Settings.HexColor));
			SetBinding(nodeTypeColorBox, nameof(ColorBox.Color), settings, nameof(Settings.TypeColor));
			SetBinding(nodeNameColorBox, nameof(ColorBox.Color), settings, nameof(Settings.NameColor));
			SetBinding(nodeValueColorBox, nameof(ColorBox.Color), settings, nameof(Settings.ValueColor));
			SetBinding(nodeIndexColorBox, nameof(ColorBox.Color), settings, nameof(Settings.IndexColor));
			SetBinding(nodeVTableColorBox, nameof(ColorBox.Color), settings, nameof(Settings.VTableColor));
			SetBinding(nodeCommentColorBox, nameof(ColorBox.Color), settings, nameof(Settings.CommentColor));
			SetBinding(nodeTextColorBox, nameof(ColorBox.Color), settings, nameof(Settings.TextColor));
			SetBinding(nodePluginColorBox, nameof(ColorBox.Color), settings, nameof(Settings.PluginColor));
		}

		private void SetTypedefinitionBindings()
		{
			SetBinding(boolTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeBool));
			SetBinding(int8TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeInt8));
			SetBinding(int16TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeInt16));
			SetBinding(int32TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeInt32));
			SetBinding(int64TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeInt64));
			SetBinding(uint8TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUInt8));
			SetBinding(uint16TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUInt16));
			SetBinding(uint32TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUInt32));
			SetBinding(uint64TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUInt64));
			SetBinding(floatTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeFloat));
			SetBinding(doubleTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeDouble));
			SetBinding(vector2TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeVector2));
			SetBinding(vector3TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeVector3));
			SetBinding(vector4TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeVector4));
			SetBinding(matrix3x3TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeMatrix3x3));
			SetBinding(matrix3x4TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeMatrix3x4));
			SetBinding(matrix4x4TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeMatrix4x4));
			SetBinding(utf8TextTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUtf8Text));
			SetBinding(utf16TextTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUtf16Text));
			SetBinding(functionPtrTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeFunctionPtr));
		}
	}
}
