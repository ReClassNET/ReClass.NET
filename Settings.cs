using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ReClassNET
{
	public class Settings
	{
		public static Settings Load(string filename)
		{
			Contract.Requires(!string.IsNullOrEmpty(filename));

			try
			{
				using (var sr = new StreamReader(filename))
				{
					return (Settings)new XmlSerializer(typeof(Settings)).Deserialize(sr);
				}
			}
			catch
			{
				return new Settings();
			}
		}

		public static void Save(Settings settings, string filename)
		{
			Contract.Requires(settings != null);
			Contract.Requires(!string.IsNullOrEmpty(filename));

			using (var sr = new StreamWriter(filename))
			{
				new XmlSerializer(typeof(Settings)).Serialize(sr, settings);
			}
		}

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color BackgroundColor { get; set; } = Color.FromArgb(255, 255, 255);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color SelectedColor { get; set; } = Color.FromArgb(240, 240, 240);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color HiddenColor { get; set; } = Color.FromArgb(240, 240, 240);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color OffsetColor { get; set; } = Color.FromArgb(255, 0, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color AddressColor { get; set; } = Color.FromArgb(0, 200, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color HexColor { get; set; } = Color.FromArgb(0, 0, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color TypeColor { get; set; } = Color.FromArgb(0, 0, 255);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color NameColor { get; set; } = Color.FromArgb(32, 32, 128);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color ValueColor { get; set; } = Color.FromArgb(255, 128, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color IndexColor { get; set; } = Color.FromArgb(32, 200, 200);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color CommentColor { get; set; } = Color.FromArgb(0, 200, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color TextColor { get; set; } = Color.FromArgb(0, 0, 255);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color VTableColor { get; set; } = Color.FromArgb(0, 255, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color FunctionColor { get; set; } = Color.FromArgb(255, 0, 255);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color CustomColor { get; set; } = Color.FromArgb(64, 128, 64);

		private static Color[] highlightColors = new Color[]
		{
			Color.Aqua, Color.Aquamarine, Color.Blue, Color.BlueViolet, Color.Chartreuse, Color.Crimson, Color.LawnGreen, Color.Magenta
		};
		[XmlIgnore]
		[Browsable(false)]
		public Color HighlightColor => highlightColors[Program.GlobalRandom.Next(highlightColors.Length)];

		[Category("Display")]
		public bool ShowAddress { get; set; } = true;

		[Category("Display")]
		public bool ShowOffset { get; set; } = true;

		[Category("Display")]
		public bool ShowText { get; set; } = true;

		[Category("Display")]
		public bool HighlightChangedValues { get; set; } = true;

		[Category("Comment")]
		public bool ShowFloat { get; set; } = true;

		[Category("Comment")]
		public bool ShowInteger { get; set; } = true;

		[Category("Comment")]
		public bool ShowPointer { get; set; } = true;

		[Category("Comment")]
		public bool ShowRTTI { get; set; } = false;

		[Category("Comment")]
		public bool ShowSymbols { get; set; } = false;

		[Category("Comment")]
		public bool ShowStrings { get; set; } = true;

		[Category("Code Generation")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public CppTypedef Typedef { get; set; } = new CppTypedef();

		[Browsable(false)]
		public string LastProcess { get; set; } = string.Empty;
	}

	public class CppTypedef
	{
		public string Hex { get; set; } = "char";

		public string Int8 { get; set; } = "int8_t";
		public string Int16 { get; set; } = "int16_t";
		public string Int32 { get; set; } = "int32_t";
		public string Int64 { get; set; } = "int64_t";

		public string UInt8 { get; set; } = "uint8_t";
		public string UInt16 { get; set; } = "uint16_t";
		public string UInt32 { get; set; } = "uint32_t";
		public string UInt64 { get; set; } = "uint64_t";

		public string Float { get; set; } = "float";
		public string Double { get; set; } = "double";

		public string Vector4 { get; set; } = "Vector4";
		public string Vector3 { get; set; } = "Vector3";
		public string Vector2 { get; set; } = "Vector2";

		public string Matrix4x4 { get; set; } = "Matrix4x4";
		public string Matrix3x4 { get; set; } = "Matrix3x4";
		public string Matrix3x3 { get; set; } = "Matrix3x3";

		public string UTF8Text { get; set; } = "char";
		public string UTF8PtrText { get; set; } = "char*";
		public string UTF16Text { get; set; } = "wchar_t"; // Should be char16_t, but this type isn't well supported at the moment.
		public string UTF16PtrText { get; set; } = "wchar_t*";
		public string UTF32Text { get; set; } = "char32_t";
		public string UTF32PtrText { get; set; } = "char32_t*";

		public string FunctionPtr { get; set; } = "void*";

		public override string ToString() => string.Empty;
	}

	public class XmlColorWrapper : IXmlSerializable
	{
		private Color color;

		public XmlColorWrapper()
			: this(Color.Empty)
		{

		}

		public XmlColorWrapper(Color color)
		{
			this.color = color;
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			color = Color.FromArgb((int)(0xFF000000 | reader.ReadElementContentAsInt()));
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteString(color.ToRgb().ToString());
		}


		public static implicit operator XmlColorWrapper(Color color)
		{
			if (color != Color.Empty)
			{
				return new XmlColorWrapper(color);
			}

			return null;
		}

		public static implicit operator Color(XmlColorWrapper wrapper)
		{
			if (wrapper != null)
			{
				return wrapper.color;
			}

			return Color.Empty;
		}
	}
}
