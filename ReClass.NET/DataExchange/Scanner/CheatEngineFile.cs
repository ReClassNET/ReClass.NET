using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;
using ReClassNET.Logger;
using ReClassNET.MemoryScanner;

namespace ReClassNET.DataExchange.Scanner
{
	public class CheatEngineFile : IScannerImport
	{
		public const string FormatName = "Cheat Engine Tables";
		public const string FileExtension = ".ct";

		private const string Version26 = "26";

		public const string XmlVersionElement = "CheatEngineTableVersion";
		public const string XmlEntriesElement = "CheatEntries";
		public const string XmlEntryElement = "CheatEntry";
		public const string XmlDescriptionElement = "Description";
		public const string XmlValueTypeElement = "VariableType";
		public const string XmlAddressElement = "Address";
		public const string XmlUnicodeElement = "Unicode";
		public const string XmlLengthElement = "Length";

		public IEnumerable<MemoryRecord> Load(string filePath, ILogger logger)
		{
			using (var stream = File.OpenRead(filePath))
			{
				var document = XDocument.Load(stream);
				if (document.Root != null)
				{
					var version = document.Root.Attribute(XmlVersionElement)?.Value;
					if (string.Compare(version, Version26, StringComparison.Ordinal) >= 0)
					{
						var entries = document.Root.Element(XmlEntriesElement);
						if (entries != null)
						{
							foreach (var entry in entries.Elements(XmlEntryElement))
							{
								var description = entry.Element(XmlDescriptionElement)?.Value.Trim() ?? string.Empty;
								if (description == "\"No description\"")
								{
									description = string.Empty;
								}
								var variableTypeStr = entry.Element(XmlValueTypeElement)?.Value.Trim() ?? string.Empty;
								var valueType = Parse(variableTypeStr, logger);

								var record = new MemoryRecord
								{
									Description = description,
									ValueType = valueType
								};

								var addressStr = entry.Element(XmlAddressElement)?.Value.Trim() ?? string.Empty;
								var addressParts = addressStr.Split('+');
								if (addressParts.Length == 2)
								{
									long.TryParse(addressParts[1], NumberStyles.HexNumber, null, out var value);
									record.AddressOrOffset = (IntPtr)value;

									record.ModuleName = addressParts[0].Trim();
								}
								else
								{
									long.TryParse(addressStr, NumberStyles.HexNumber, null, out var value);
									record.AddressOrOffset = (IntPtr)value;
								}

								if (valueType == ScanValueType.ArrayOfBytes || valueType == ScanValueType.String)
								{
									var lengthStr = entry.Element(XmlLengthElement)?.Value ?? string.Empty;
									int.TryParse(lengthStr, NumberStyles.Integer, null, out var valueLength);

									record.ValueLength = Math.Max(1, valueLength);

									if (valueType == ScanValueType.String)
									{
										var isUnicode = (entry.Element(XmlUnicodeElement)?.Value ?? string.Empty) == "1";

										record.Encoding = isUnicode ? Encoding.Unicode : Encoding.UTF8;
									}
								}

								yield return record;
							}
						}
					}
				}
			}
		}

		private static ScanValueType Parse(string value, ILogger logger)
		{
			switch (value)
			{
				case "Byte":
					return ScanValueType.Byte;
				case "2 Bytes":
					return ScanValueType.Short;
				case "4 Bytes":
					return ScanValueType.Integer;
				case "8 Bytes":
					return ScanValueType.Long;
				case "Float":
					return ScanValueType.Float;
				case "Double":
					return ScanValueType.Double;
				case "String":
					return ScanValueType.String;
				case "Array of byte":
					return ScanValueType.ArrayOfBytes;
				default:
					logger?.Log(LogLevel.Warning, $"Unknown value type: {value}");

					return ScanValueType.Integer;
			}
		}
	}
}
