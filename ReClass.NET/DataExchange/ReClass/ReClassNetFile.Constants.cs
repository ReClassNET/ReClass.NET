namespace ReClassNET.DataExchange.ReClass
{
	public partial class ReClassNetFile
	{
		public const string FormatName = "ReClass.NET File";
		public const string FileExtension = ".rcnet";
		public const string FileExtensionId = "rcnetfile";

		private const uint FileVersion = 0x00010001;
		private const uint FileVersionCriticalMask = 0xFFFF0000;

		private const string DataFileName = "Data.xml";

		private const string SerializationClassName = "__Serialization_Class__";

		public const string XmlRootElement = "reclass";
		public const string XmlCustomDataElement = "custom_data";
		public const string XmlTypeMappingElement = "type_mapping";
		public const string XmlEnumsElement = "enums";
		public const string XmlEnumElement = "enum";
		public const string XmlClassesElement = "classes";
		public const string XmlClassElement = "class";
		public const string XmlNodeElement = "node";
		public const string XmlMethodElement = "method";
		public const string XmlVersionAttribute = "version";
		public const string XmlPlatformAttribute = "type";
		public const string XmlUuidAttribute = "uuid";
		public const string XmlNameAttribute = "name";
		public const string XmlCommentAttribute = "comment";
		public const string XmlHiddenAttribute = "hidden";
		public const string XmlAddressAttribute = "address";
		public const string XmlTypeAttribute = "type";
		public const string XmlReferenceAttribute = "reference";
		public const string XmlCountAttribute = "count";
		public const string XmlBitsAttribute = "bits";
		public const string XmlLengthAttribute = "length";
		public const string XmlSizeAttribute = "size";
		public const string XmlSignatureAttribute = "signature";
		public const string XmlFlagsAttribute = "flags";
		public const string XmlItemElement = "item";
		public const string XmlValueAttribute = "value";
	}
}
