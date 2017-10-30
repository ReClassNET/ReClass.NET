using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ReClassNET.Util
{
	internal sealed class SettingsSerializer
	{
		private const string XmlRootElement = "Settings";
		private const string XmlGeneralElement = "General";
		private const string XmlDisplayElement = "Display";
		private const string XmlColorsElement = "Colors";
		private const string XmlTypeDefinitionsElement = "TypeDefinitions";
		private const string XmlCustomDataElement = "CustomData";

		#region Read Settings

		public static Settings Load()
		{
			EnsureSettingsDirectoryAvailable();

			var settings = new Settings();

			try
			{
				var path = Path.Combine(PathUtil.SettingsFolderPath, Constants.SettingsFile);

				using (var sr = new StreamReader(path))
				{
					var document = XDocument.Load(sr);
					var root = document.Root;

					var general = root?.Element(XmlGeneralElement);
					if (general != null)
					{
						TryRead(general, nameof(settings.LastProcess), e => settings.LastProcess = ToString(e));
						TryRead(general, nameof(settings.StayOnTop), e => settings.StayOnTop = ToBool(e));
					}
					var display = root?.Element(XmlDisplayElement);
					if (display != null)
					{
						TryRead(display, nameof(settings.ShowNodeAddress), e => settings.ShowNodeAddress = ToBool(e));
						TryRead(display, nameof(settings.ShowNodeOffset), e => settings.ShowNodeOffset = ToBool(e));
						TryRead(display, nameof(settings.ShowNodeText), e => settings.ShowNodeText = ToBool(e));
						TryRead(display, nameof(settings.HighlightChangedValues), e => settings.HighlightChangedValues = ToBool(e));
						TryRead(display, nameof(settings.ShowCommentFloat), e => settings.ShowCommentFloat = ToBool(e));
						TryRead(display, nameof(settings.ShowCommentInteger), e => settings.ShowCommentInteger = ToBool(e));
						TryRead(display, nameof(settings.ShowCommentPointer), e => settings.ShowCommentPointer = ToBool(e));
						TryRead(display, nameof(settings.ShowCommentRtti), e => settings.ShowCommentRtti = ToBool(e));
						TryRead(display, nameof(settings.ShowCommentSymbol), e => settings.ShowCommentSymbol = ToBool(e));
						TryRead(display, nameof(settings.ShowCommentString), e => settings.ShowCommentString = ToBool(e));
						TryRead(display, nameof(settings.ShowCommentPluginInfo), e => settings.ShowCommentPluginInfo = ToBool(e));
					}
					var colors = root?.Element(XmlColorsElement);
					if (colors != null)
					{
						TryRead(colors, nameof(settings.BackgroundColor), e => settings.BackgroundColor = ToColor(e));
						TryRead(colors, nameof(settings.SelectedColor), e => settings.SelectedColor = ToColor(e));
						TryRead(colors, nameof(settings.HiddenColor), e => settings.HiddenColor = ToColor(e));
						TryRead(colors, nameof(settings.OffsetColor), e => settings.OffsetColor = ToColor(e));
						TryRead(colors, nameof(settings.AddressColor), e => settings.AddressColor = ToColor(e));
						TryRead(colors, nameof(settings.HexColor), e => settings.HexColor = ToColor(e));
						TryRead(colors, nameof(settings.TypeColor), e => settings.TypeColor = ToColor(e));
						TryRead(colors, nameof(settings.NameColor), e => settings.NameColor = ToColor(e));
						TryRead(colors, nameof(settings.ValueColor), e => settings.ValueColor = ToColor(e));
						TryRead(colors, nameof(settings.IndexColor), e => settings.IndexColor = ToColor(e));
						TryRead(colors, nameof(settings.CommentColor), e => settings.CommentColor = ToColor(e));
						TryRead(colors, nameof(settings.TextColor), e => settings.TextColor = ToColor(e));
						TryRead(colors, nameof(settings.VTableColor), e => settings.VTableColor = ToColor(e));
					}
					var typeDefinitions = root?.Element(XmlTypeDefinitionsElement);
					if (typeDefinitions != null)
					{
						TryRead(typeDefinitions, nameof(settings.TypePadding), e => settings.TypePadding = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeBool), e => settings.TypeBool = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeInt8), e => settings.TypeInt8 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeInt16), e => settings.TypeInt16 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeInt32), e => settings.TypeInt32 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeInt64), e => settings.TypeInt64 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUInt8), e => settings.TypeUInt8 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUInt16), e => settings.TypeUInt16 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUInt32), e => settings.TypeUInt32 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUInt64), e => settings.TypeUInt64 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeFloat), e => settings.TypeFloat = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeDouble), e => settings.TypeDouble = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeVector2), e => settings.TypeVector2 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeVector3), e => settings.TypeVector3 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeVector4), e => settings.TypeVector4 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeMatrix3x3), e => settings.TypeMatrix3x3 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeMatrix3x4), e => settings.TypeMatrix3x4 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeMatrix4x4), e => settings.TypeMatrix4x4 = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUTF8Text), e => settings.TypeUTF8Text = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUTF8TextPtr), e => settings.TypeUTF8TextPtr = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUTF16Text), e => settings.TypeUTF16Text = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUTF16TextPtr), e => settings.TypeUTF16TextPtr = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUTF32Text), e => settings.TypeUTF32Text = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeUTF32TextPtr), e => settings.TypeUTF32TextPtr = ToString(e));
						TryRead(typeDefinitions, nameof(settings.TypeFunctionPtr), e => settings.TypeFunctionPtr = ToString(e));
					}
					var customData = root?.Element(XmlCustomDataElement);
					if (customData != null)
					{
						foreach (var kv in ToDictionary(customData))
						{
							settings.CustomData[kv.Key] = kv.Value;
						}
					}
				}
			}
			catch
			{

			}

			return settings;
		}

		private static void TryRead(XContainer element, string name, Action<XElement> iff)
		{
			Contract.Requires(element != null);
			Contract.Requires(name != null);
			Contract.Requires(iff != null);

			var target = element.Element(name);
			if (target != null)
			{
				iff(target);
			}
		}

		private static bool ToBool(XElement value) => bool.Parse(value.Value);
		private static string ToString(XElement value) => value.Value;
		private static Color ToColor(XElement value) => Color.FromArgb((int)(0xFF000000 | int.Parse(value.Value, NumberStyles.HexNumber)));
		private static Dictionary<string, string> ToDictionary(XContainer value) => value.Elements().ToDictionary(e => e.Name.ToString(), e => e.Value);

		#endregion

		#region Write Settings

		public static void Save(Settings settings)
		{
			Contract.Requires(settings != null);

			EnsureSettingsDirectoryAvailable();

			var path = Path.Combine(PathUtil.SettingsFolderPath, Constants.SettingsFile);

			using (var sw = new StreamWriter(path))
			{
				var document = new XDocument(
					new XComment($"{Constants.ApplicationName} {Constants.ApplicationVersion} by {Constants.Author}"),
					new XComment($"Website: {Constants.HomepageUrl}"),
					new XElement(
						XmlRootElement,
						new XElement(
							XmlGeneralElement,
							ToXml(nameof(settings.LastProcess), settings.LastProcess),
							ToXml(nameof(settings.StayOnTop), settings.StayOnTop)
						),
						new XElement(
							XmlDisplayElement,
							ToXml(nameof(settings.ShowNodeAddress), settings.ShowNodeAddress),
							ToXml(nameof(settings.ShowNodeOffset), settings.ShowNodeOffset),
							ToXml(nameof(settings.ShowNodeText), settings.ShowNodeText),
							ToXml(nameof(settings.HighlightChangedValues), settings.HighlightChangedValues),
							ToXml(nameof(settings.ShowCommentFloat), settings.ShowCommentFloat),
							ToXml(nameof(settings.ShowCommentInteger), settings.ShowCommentInteger),
							ToXml(nameof(settings.ShowCommentPointer), settings.ShowCommentPointer),
							ToXml(nameof(settings.ShowCommentRtti), settings.ShowCommentRtti),
							ToXml(nameof(settings.ShowCommentSymbol), settings.ShowCommentSymbol),
							ToXml(nameof(settings.ShowCommentString), settings.ShowCommentString),
							ToXml(nameof(settings.ShowCommentPluginInfo), settings.ShowCommentPluginInfo)
						),
						new XElement(
							XmlColorsElement,
							ToXml(nameof(settings.BackgroundColor), settings.BackgroundColor),
							ToXml(nameof(settings.SelectedColor), settings.SelectedColor),
							ToXml(nameof(settings.HiddenColor), settings.HiddenColor),
							ToXml(nameof(settings.OffsetColor), settings.OffsetColor),
							ToXml(nameof(settings.AddressColor), settings.AddressColor),
							ToXml(nameof(settings.HexColor), settings.HexColor),
							ToXml(nameof(settings.TypeColor), settings.TypeColor),
							ToXml(nameof(settings.NameColor), settings.NameColor),
							ToXml(nameof(settings.ValueColor), settings.ValueColor),
							ToXml(nameof(settings.IndexColor), settings.IndexColor),
							ToXml(nameof(settings.CommentColor), settings.CommentColor),
							ToXml(nameof(settings.TextColor), settings.TextColor),
							ToXml(nameof(settings.VTableColor), settings.VTableColor)
						),
						new XElement(
							XmlTypeDefinitionsElement,
							ToXml(nameof(settings.TypePadding), settings.TypePadding),
							ToXml(nameof(settings.TypeBool), settings.TypeBool),
							ToXml(nameof(settings.TypeInt8), settings.TypeInt8),
							ToXml(nameof(settings.TypeInt16), settings.TypeInt16),
							ToXml(nameof(settings.TypeInt32), settings.TypeInt32),
							ToXml(nameof(settings.TypeInt64), settings.TypeInt64),
							ToXml(nameof(settings.TypeUInt8), settings.TypeUInt8),
							ToXml(nameof(settings.TypeUInt16), settings.TypeUInt16),
							ToXml(nameof(settings.TypeUInt32), settings.TypeUInt32),
							ToXml(nameof(settings.TypeUInt64), settings.TypeUInt64),
							ToXml(nameof(settings.TypeFloat), settings.TypeFloat),
							ToXml(nameof(settings.TypeDouble), settings.TypeDouble),
							ToXml(nameof(settings.TypeVector2), settings.TypeVector2),
							ToXml(nameof(settings.TypeVector3), settings.TypeVector3),
							ToXml(nameof(settings.TypeVector4), settings.TypeVector4),
							ToXml(nameof(settings.TypeMatrix3x3), settings.TypeMatrix3x3),
							ToXml(nameof(settings.TypeMatrix3x4), settings.TypeMatrix3x4),
							ToXml(nameof(settings.TypeMatrix4x4), settings.TypeMatrix4x4),
							ToXml(nameof(settings.TypeUTF8Text), settings.TypeUTF8Text),
							ToXml(nameof(settings.TypeUTF8TextPtr), settings.TypeUTF8TextPtr),
							ToXml(nameof(settings.TypeUTF16Text), settings.TypeUTF16Text),
							ToXml(nameof(settings.TypeUTF16TextPtr), settings.TypeUTF16TextPtr),
							ToXml(nameof(settings.TypeUTF32Text), settings.TypeUTF32Text),
							ToXml(nameof(settings.TypeUTF32TextPtr), settings.TypeUTF32TextPtr),
							ToXml(nameof(settings.TypeFunctionPtr), settings.TypeFunctionPtr)
						),
						ToXml(XmlCustomDataElement, settings.CustomData)
					)
				);

				document.Save(sw);
			}
		}

		private static XElement ToXml(string name, bool value) => new XElement(name, value);
		private static XElement ToXml(string name, string value) => new XElement(name, value);
		private static XElement ToXml(string name, Color value) => new XElement(name, $"{value.ToRgb():X6}");
		private static XElement ToXml(string name, Dictionary<string, string> value) => new XElement(name, value.Select(kv => new XElement(kv.Key, kv.Value)));

		#endregion

		private static void EnsureSettingsDirectoryAvailable()
		{
			try
			{
				if (Directory.Exists(PathUtil.SettingsFolderPath) == false)
				{
					Directory.CreateDirectory(PathUtil.SettingsFolderPath);
				}
			}
			catch (Exception)
			{

			}
		}
	}
}
