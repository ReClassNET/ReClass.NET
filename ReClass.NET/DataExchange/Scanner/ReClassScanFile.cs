using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.MemoryScanner;

namespace ReClassNET.DataExchange.Scanner
{
	public class ReClassScanFile : IScannerImport, IScannerExport
	{
		public const string FormatName = "ReClass.NET Scanner File";
		public const string FileExtension = ".rcnetscan";

		private const string Version1 = "1";

		private const string DataFileName = "Data.xml";

		public const string XmlRootElement = "records";
		public const string XmlRecordElement = "record";

		public const string XmlVersionAttribute = "version";
		public const string XmlPlatformAttribute = "platform";
		public const string XmlValueTypeAttribute = "type";
		public const string XmlAddressAttribute = "address";
		public const string XmlModuleAttribute = "module";
		public const string XmlDescriptionAttribute = "description";
		public const string XmlValueLengthAttribute = "length";
		public const string XmlEncodingAttribute = "encoding";

		public IEnumerable<MemoryRecord> Load(string filePath, ILogger logger)
		{
			using (var fs = new FileStream(filePath, FileMode.Open))
			{
				using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
				{
					var dataEntry = archive.GetEntry(DataFileName);
					if (dataEntry == null)
					{
						throw new FormatException();
					}
					using (var entryStream = dataEntry.Open())
					{
						var document = XDocument.Load(entryStream);
						if (document.Root == null)
						{
							logger.Log(LogLevel.Error, "File has not the correct format.");
							yield break;
						}

						//var version = document.Root.Attribute(XmlVersionAttribute)?.Value;
						var platform = document.Root.Attribute(XmlPlatformAttribute)?.Value;
						if (platform != Constants.Platform)
						{
							logger.Log(LogLevel.Warning, $"The platform of the file ({platform}) doesn't match the program platform ({Constants.Platform}).");
						}

						foreach (var element in document.Root.Elements(XmlRecordElement))
						{
							var valueTypeStr = element.Attribute(XmlValueTypeAttribute)?.Value ?? string.Empty;

							if (!Enum.TryParse<ScanValueType>(valueTypeStr, out var valueType))
							{
								logger?.Log(LogLevel.Warning, $"Unknown value type: {valueTypeStr}");
								continue;
							}

							var description = element.Attribute(XmlDescriptionAttribute)?.Value ?? string.Empty;

							var addressStr = element.Attribute(XmlAddressAttribute)?.Value ?? string.Empty;
							var moduleName = element.Attribute(XmlModuleAttribute)?.Value ?? string.Empty;

							long.TryParse(addressStr, NumberStyles.HexNumber, null, out var address);

							var record = new MemoryRecord
							{
								Description = description,
								AddressOrOffset = (IntPtr)address,
								ValueType = valueType
							};

							if (!string.IsNullOrEmpty(moduleName))
							{
								record.ModuleName = moduleName;
							}

							if (valueType == ScanValueType.ArrayOfBytes || valueType == ScanValueType.String)
							{
								var lengthStr = element.Attribute(XmlValueLengthAttribute)?.Value ?? string.Empty;
								int.TryParse(lengthStr, NumberStyles.Integer, null, out var valueLength);

								record.ValueLength = Math.Max(1, valueLength);

								if (valueType == ScanValueType.String)
								{
									switch (element.Attribute(XmlEncodingAttribute)?.Value ?? string.Empty)
									{
										default:
											record.Encoding = Encoding.UTF8;
											break;
										case "UTF16":
											record.Encoding = Encoding.Unicode;
											break;
										case "UTF32":
											record.Encoding = Encoding.UTF32;
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

		public void Save(IEnumerable<MemoryRecord> records, string filePath, ILogger logger)
		{
			using (var fs = new FileStream(filePath, FileMode.Create))
			{
				using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
				{
					var dataEntry = archive.CreateEntry(DataFileName);
					using (var entryStream = dataEntry.Open())
					{
						var document = new XDocument(
							new XComment($"{Constants.ApplicationName} Scanner {Constants.ApplicationVersion} by {Constants.Author}"),
							new XComment($"Website: {Constants.HomepageUrl}"),
							new XElement(
								XmlRootElement,
								new XAttribute(XmlVersionAttribute, Version1),
								new XAttribute(XmlPlatformAttribute, Constants.Platform),
								records.Select(r =>
								{
									var temp = new XElement(
										XmlRecordElement,
										new XAttribute(XmlValueTypeAttribute, r.ValueType.ToString()),
										new XAttribute(XmlDescriptionAttribute, r.Description ?? string.Empty),
										new XAttribute(XmlAddressAttribute, r.AddressOrOffset.ToString(Constants.AddressHexFormat))
									);
									if (r.IsRelativeAddress)
									{
										temp.SetAttributeValue(XmlModuleAttribute, r.ModuleName);
									}
									if (r.ValueType == ScanValueType.ArrayOfBytes || r.ValueType == ScanValueType.String)
									{
										temp.SetAttributeValue(XmlValueLengthAttribute, r.ValueLength);
										if (r.ValueType == ScanValueType.String)
										{
											temp.SetAttributeValue(XmlEncodingAttribute, r.Encoding.IsSameCodePage(Encoding.UTF8) ? "UTF8" : r.Encoding.IsSameCodePage(Encoding.Unicode) ? "UTF16" : "UTF32");
										}
									}
									return temp;
								})
							)
						);

						document.Save(entryStream);
					}
				}
			}
		}
	}
}
