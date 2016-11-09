namespace ReClassNET.DataExchange
{
	public enum SchemaType
	{
		None,
		Padding,

		Array,
		ClassPtrArray,
		ClassInstance,
		Class,
		ClassPtr,
		Double,
		Float,
		FunctionPtr,
		Hex8,
		Hex16,
		Hex32,
		Hex64,
		Int8,
		Int16,
		Int32,
		Int64,
		Matrix3x3,
		Matrix3x4,
		Matrix4x4,
		UInt8,
		UInt16,
		UInt32,
		UInt64,
		UTF8Text,
		UTF8TextPtr,
		UTF16Text,
		UTF16TextPtr,
		UTF32Text,
		UTF32TextPtr,
		Vector2,
		Vector3,
		Vector4,
		VTable,
		VMethod,
		BitField,

		Custom
	}
}
