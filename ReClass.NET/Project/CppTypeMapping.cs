using System.Xml.Linq;
using ReClassNET.Util;

namespace ReClassNET.Project
{
	public class CppTypeMapping
	{
		public string TypeBool { get; set; } = "bool";

		public string TypeInt8 { get; set; } = "int8_t";
		public string TypeInt16 { get; set; } = "int16_t";
		public string TypeInt32 { get; set; } = "int32_t";
		public string TypeInt64 { get; set; } = "int64_t";

		public string TypeUInt8 { get; set; } = "uint8_t";
		public string TypeUInt16 { get; set; } = "uint16_t";
		public string TypeUInt32 { get; set; } = "uint32_t";
		public string TypeUInt64 { get; set; } = "uint64_t";

		public string TypeFloat { get; set; } = "float";
		public string TypeDouble { get; set; } = "double";

		public string TypeVector2 { get; set; } = "Vector2";
		public string TypeVector3 { get; set; } = "Vector3";
		public string TypeVector4 { get; set; } = "Vector4";

		public string TypeMatrix3x3 { get; set; } = "Matrix3x3";
		public string TypeMatrix3x4 { get; set; } = "Matrix3x4";
		public string TypeMatrix4x4 { get; set; } = "Matrix4x4";

		public string TypeUtf8Text { get; set; } = "char";
		public string TypeUtf16Text { get; set; } = "wchar_t"; // Should be char16_t, but this type isn't well supported at the moment.
		public string TypeUtf32Text { get; set; } = "char32_t";

		public string TypeFunctionPtr { get; set; } = "void*";

		internal XElement Serialize(string name)
		{
			return new XElement(
				name,
				XElementSerializer.ToXml(nameof(TypeBool), TypeBool),
				XElementSerializer.ToXml(nameof(TypeInt8), TypeInt8),
				XElementSerializer.ToXml(nameof(TypeInt16), TypeInt16),
				XElementSerializer.ToXml(nameof(TypeInt32), TypeInt32),
				XElementSerializer.ToXml(nameof(TypeInt64), TypeInt64),
				XElementSerializer.ToXml(nameof(TypeUInt8), TypeUInt8),
				XElementSerializer.ToXml(nameof(TypeUInt16), TypeUInt16),
				XElementSerializer.ToXml(nameof(TypeUInt32), TypeUInt32),
				XElementSerializer.ToXml(nameof(TypeUInt64), TypeUInt64),
				XElementSerializer.ToXml(nameof(TypeFloat), TypeFloat),
				XElementSerializer.ToXml(nameof(TypeDouble), TypeDouble),
				XElementSerializer.ToXml(nameof(TypeVector2), TypeVector2),
				XElementSerializer.ToXml(nameof(TypeVector3), TypeVector3),
				XElementSerializer.ToXml(nameof(TypeVector4), TypeVector4),
				XElementSerializer.ToXml(nameof(TypeMatrix3x3), TypeMatrix3x3),
				XElementSerializer.ToXml(nameof(TypeMatrix3x4), TypeMatrix3x4),
				XElementSerializer.ToXml(nameof(TypeMatrix4x4), TypeMatrix4x4),
				XElementSerializer.ToXml(nameof(TypeUtf8Text), TypeUtf8Text),
				XElementSerializer.ToXml(nameof(TypeUtf16Text), TypeUtf16Text),
				XElementSerializer.ToXml(nameof(TypeUtf32Text), TypeUtf32Text),
				XElementSerializer.ToXml(nameof(TypeFunctionPtr), TypeFunctionPtr)
			);
		}

		internal void Deserialize(XElement element)
		{
			XElementSerializer.TryRead(element, nameof(TypeBool), e => TypeBool = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeInt8), e => TypeInt8 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeInt16), e => TypeInt16 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeInt32), e => TypeInt32 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeInt64), e => TypeInt64 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeUInt8), e => TypeUInt8 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeUInt16), e => TypeUInt16 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeUInt32), e => TypeUInt32 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeUInt64), e => TypeUInt64 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeFloat), e => TypeFloat = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeDouble), e => TypeDouble = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeVector2), e => TypeVector2 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeVector3), e => TypeVector3 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeVector4), e => TypeVector4 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeMatrix3x3), e => TypeMatrix3x3 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeMatrix3x4), e => TypeMatrix3x4 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeMatrix4x4), e => TypeMatrix4x4 = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeUtf8Text), e => TypeUtf8Text = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeUtf16Text), e => TypeUtf16Text = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeUtf32Text), e => TypeUtf32Text = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(TypeFunctionPtr), e => TypeFunctionPtr = XElementSerializer.ToString(e));
		}
	}
}
