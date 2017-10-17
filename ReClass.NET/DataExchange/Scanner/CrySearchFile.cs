using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using ReClassNET.Logger;
using ReClassNET.MemoryScanner;

namespace ReClassNET.DataExchange.Scanner
{
	public class CrySearchFile : IScannerImport
	{
		public const string FormatName = "CrySearch Address Tables";
		public const string FileExtension = ".csat";

		private const string Version3 = "3.0";

		public const string XmlVersionElement = "CrySearchVersion";
		public const string XmlEntriesElement = "Entries";
		public const string XmlItemElement = "item";
		public const string XmlDescriptionElement = "Description";
		public const string XmlValueTypeElement = "ValueType";
		public const string XmlAddressElement = "Address";
		public const string XmlModuleNameElement = "ModuleName";
		public const string XmlIsRelativeElement = "IsRelative";
		public const string XmlSizeElement = "Size";

		public const string XmlValueAttribute = "value";

		public IEnumerable<MemoryRecord> Load(string filePath, ILogger logger)
		{
			var document = XDocument.Load(filePath);
			if (document.Root != null)
			{
				var version = document.Root.Element(XmlVersionElement)?.Value;
				if (string.Compare(version, Version3, StringComparison.Ordinal) >= 0)
				{
					var entries = document.Root.Element(XmlEntriesElement);
					if (entries != null)
					{
						foreach (var entry in entries.Elements(XmlItemElement))
						{
							var description = entry.Element(XmlDescriptionElement)?.Value.Trim() ?? string.Empty;
							var valueTypeStr = entry.Element(XmlValueTypeElement)?.Attribute(XmlValueAttribute)?.Value.Trim() ?? string.Empty;
							var addressStr = entry.Element(XmlAddressElement)?.Attribute(XmlValueAttribute)?.Value.Trim() ?? string.Empty;
							var moduleName = entry.Element(XmlModuleNameElement)?.Value.Trim() ?? string.Empty;

							long.TryParse(addressStr, NumberStyles.Number, null, out var value);
							var valueType = Parse(valueTypeStr, logger);

							var record = new MemoryRecord
							{
								AddressOrOffset = (IntPtr)value,
								Description = description,
								ValueType = valueType
							};

							if ((entry.Element(XmlIsRelativeElement)?.Attribute(XmlValueAttribute)?.Value.Trim() ?? string.Empty) == "1" && !string.IsNullOrEmpty(moduleName))
							{
								record.ModuleName = moduleName;
							}

							if (valueType == ScanValueType.ArrayOfBytes || valueType == ScanValueType.String)
							{
								var lengthStr = (entry.Element(XmlSizeElement)?.Attribute(XmlValueAttribute)?.Value.Trim() ?? string.Empty);
								int.TryParse(lengthStr, NumberStyles.Integer, null, out var valueLength);

								record.ValueLength = Math.Max(1, valueLength);

								if (valueType == ScanValueType.String)
								{
									switch (valueTypeStr)
									{
										default:
										case "8":
											record.Encoding = Encoding.UTF8;
											break;
										case "9":
											record.Encoding = Encoding.Unicode;
											break;
									}
								}
							}

							yield return record;
						}
					}
				}
			}
		}

		private static ScanValueType Parse(string value, ILogger logger)
		{
			switch (value)
			{
				case "1":
					return ScanValueType.Byte;
				case "2":
					return ScanValueType.Short;
				case "3":
					return ScanValueType.Integer;
				case "4":
					return ScanValueType.Long;
				case "5":
					return ScanValueType.Float;
				case "6":
					return ScanValueType.Double;
				case "7":
					return ScanValueType.ArrayOfBytes;
				case "8":
				case "9":
					return ScanValueType.String;
				default:
					logger?.Log(LogLevel.Warning, $"Unknown value type: {value}");

					return ScanValueType.Integer;
			}
		}
	}
}
