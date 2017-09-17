using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;
using ReClassNET.Logger;
using ReClassNET.MemorySearcher;

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
									record.Address = (IntPtr)value;

									record.ModuleName = addressParts[0].Trim();
								}
								else
								{
									long.TryParse(addressStr, NumberStyles.HexNumber, null, out var value);
									record.Address = (IntPtr)value;
								}

								if (valueType == SearchValueType.ArrayOfBytes || valueType == SearchValueType.String)
								{
									var lengthStr = entry.Element(XmlLengthElement)?.Value ?? string.Empty;
									int.TryParse(lengthStr, NumberStyles.Integer, null, out var valueLength);

									record.ValueLength = Math.Max(1, valueLength);

									if (valueType == SearchValueType.String)
									{
										var isUnicode = (entry.Element(XmlUnicodeElement)?.Value ?? string.Empty) == "1";

										if (isUnicode)
										{
											record.Encoding = Encoding.Unicode;
										}
										else
										{
											record.Encoding = Encoding.UTF8;
										}
									}
								}

								yield return record;
							}
						}
					}
				}
			}
		}

		private static SearchValueType Parse(string value, ILogger logger)
		{
			switch (value)
			{
				case "Byte":
					return SearchValueType.Byte;
				case "2 Bytes":
					return SearchValueType.Short;
				case "4 Bytes":
					return SearchValueType.Integer;
				case "8 Bytes":
					return SearchValueType.Long;
				case "Float":
					return SearchValueType.Float;
				case "Double":
					return SearchValueType.Double;
				case "String":
					return SearchValueType.String;
				case "Array of byte":
					return SearchValueType.ArrayOfBytes;
				default:
					logger?.Log(LogLevel.Warning, $"Unknown value type: {value}");

					return SearchValueType.Integer;
			}
		}
	}
}
