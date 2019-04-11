using System.Drawing;

namespace ReClassNET.UI
{
	public class Icons
	{
		public static int Dimensions { get; } = DpiUtil.ScaleIntX(16);

		public static Image OpenCloseOpen { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Open_Icon);
		public static Image OpenCloseClosed { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Closed_Icon);
		public static Image Delete { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Button_Delete);
		public static Image DropArrow { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Button_Drop_Down);
		public static Image Class { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Class_Type);
		public static Image Enum { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Enum_Type);
		public static Image Array { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Array_Type);
		public static Image Union => Array;
		public static Image LeftArrow { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Left_Button);
		public static Image RightArrow { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Right_Button);
		public static Image Change { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Exchange_Button);
		public static Image Unsigned { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Unsigned_Type);
		public static Image Signed { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Signed_Type);
		public static Image Float { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Float_Type);
		public static Image Double { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Double_Type);
		public static Image Vector { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Vector_Type);
		public static Image Matrix { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Matrix_Type);
		public static Image Text { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Text_Type);
		public static Image Pointer { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Pointer_Type);
		public static Image Function { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Function_Type);
		public static Image VTable { get; } = DpiUtil.ScaleImage(Properties.Resources.B16x16_Interface_Type);
	}
}
