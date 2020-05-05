using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Linq;

namespace ReClassNET.Util
{
	internal sealed class SettingsSerializer
	{
		private const string XmlRootElement = "Settings";
		private const string XmlGeneralElement = "General";
		private const string XmlDisplayElement = "Display";
		private const string XmlColorsElement = "Colors";
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
						XElementSerializer.TryRead(general, nameof(settings.LastProcess), e => settings.LastProcess = XElementSerializer.ToString(e));
						XElementSerializer.TryRead(general, nameof(settings.StayOnTop), e => settings.StayOnTop = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(general, nameof(settings.RunAsAdmin), e => settings.RunAsAdmin = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(general, nameof(settings.RandomizeWindowTitle), e => settings.RandomizeWindowTitle = XElementSerializer.ToBool(e));
					}
					var display = root?.Element(XmlDisplayElement);
					if (display != null)
					{
						XElementSerializer.TryRead(display, nameof(settings.ShowNodeAddress), e => settings.ShowNodeAddress = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowNodeOffset), e => settings.ShowNodeOffset = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowNodeText), e => settings.ShowNodeText = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.HighlightChangedValues), e => settings.HighlightChangedValues = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowCommentFloat), e => settings.ShowCommentFloat = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowCommentInteger), e => settings.ShowCommentInteger = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowCommentPointer), e => settings.ShowCommentPointer = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowCommentRtti), e => settings.ShowCommentRtti = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowCommentSymbol), e => settings.ShowCommentSymbol = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowCommentString), e => settings.ShowCommentString = XElementSerializer.ToBool(e));
						XElementSerializer.TryRead(display, nameof(settings.ShowCommentPluginInfo), e => settings.ShowCommentPluginInfo = XElementSerializer.ToBool(e));
					}
					var colors = root?.Element(XmlColorsElement);
					if (colors != null)
					{
						XElementSerializer.TryRead(colors, nameof(settings.BackgroundColor), e => settings.BackgroundColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.SelectedColor), e => settings.SelectedColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.HiddenColor), e => settings.HiddenColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.OffsetColor), e => settings.OffsetColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.AddressColor), e => settings.AddressColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.HexColor), e => settings.HexColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.TypeColor), e => settings.TypeColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.NameColor), e => settings.NameColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.ValueColor), e => settings.ValueColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.IndexColor), e => settings.IndexColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.CommentColor), e => settings.CommentColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.TextColor), e => settings.TextColor = XElementSerializer.ToColor(e));
						XElementSerializer.TryRead(colors, nameof(settings.VTableColor), e => settings.VTableColor = XElementSerializer.ToColor(e));
					}
					var customData = root?.Element(XmlCustomDataElement);
					if (customData != null)
					{
						settings.CustomData.Deserialize(customData);
					}
				}
			}
			catch
			{

			}

			return settings;
		}

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
							XElementSerializer.ToXml(nameof(settings.LastProcess), settings.LastProcess),
							XElementSerializer.ToXml(nameof(settings.StayOnTop), settings.StayOnTop),
							XElementSerializer.ToXml(nameof(settings.RunAsAdmin), settings.RunAsAdmin),
							XElementSerializer.ToXml(nameof(settings.RandomizeWindowTitle), settings.RandomizeWindowTitle)
						),
						new XElement(
							XmlDisplayElement,
							XElementSerializer.ToXml(nameof(settings.ShowNodeAddress), settings.ShowNodeAddress),
							XElementSerializer.ToXml(nameof(settings.ShowNodeOffset), settings.ShowNodeOffset),
							XElementSerializer.ToXml(nameof(settings.ShowNodeText), settings.ShowNodeText),
							XElementSerializer.ToXml(nameof(settings.HighlightChangedValues), settings.HighlightChangedValues),
							XElementSerializer.ToXml(nameof(settings.ShowCommentFloat), settings.ShowCommentFloat),
							XElementSerializer.ToXml(nameof(settings.ShowCommentInteger), settings.ShowCommentInteger),
							XElementSerializer.ToXml(nameof(settings.ShowCommentPointer), settings.ShowCommentPointer),
							XElementSerializer.ToXml(nameof(settings.ShowCommentRtti), settings.ShowCommentRtti),
							XElementSerializer.ToXml(nameof(settings.ShowCommentSymbol), settings.ShowCommentSymbol),
							XElementSerializer.ToXml(nameof(settings.ShowCommentString), settings.ShowCommentString),
							XElementSerializer.ToXml(nameof(settings.ShowCommentPluginInfo), settings.ShowCommentPluginInfo)
						),
						new XElement(
							XmlColorsElement,
							XElementSerializer.ToXml(nameof(settings.BackgroundColor), settings.BackgroundColor),
							XElementSerializer.ToXml(nameof(settings.SelectedColor), settings.SelectedColor),
							XElementSerializer.ToXml(nameof(settings.HiddenColor), settings.HiddenColor),
							XElementSerializer.ToXml(nameof(settings.OffsetColor), settings.OffsetColor),
							XElementSerializer.ToXml(nameof(settings.AddressColor), settings.AddressColor),
							XElementSerializer.ToXml(nameof(settings.HexColor), settings.HexColor),
							XElementSerializer.ToXml(nameof(settings.TypeColor), settings.TypeColor),
							XElementSerializer.ToXml(nameof(settings.NameColor), settings.NameColor),
							XElementSerializer.ToXml(nameof(settings.ValueColor), settings.ValueColor),
							XElementSerializer.ToXml(nameof(settings.IndexColor), settings.IndexColor),
							XElementSerializer.ToXml(nameof(settings.CommentColor), settings.CommentColor),
							XElementSerializer.ToXml(nameof(settings.TextColor), settings.TextColor),
							XElementSerializer.ToXml(nameof(settings.VTableColor), settings.VTableColor)
						),
						settings.CustomData.Serialize(XmlCustomDataElement)
					)
				);

				document.Save(sw);
			}
		}

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
