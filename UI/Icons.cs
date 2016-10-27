using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET
{
	public class Icons
	{
		private static Image openCloseOpenIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Open_Icon);
		private static Image openCloseClosedIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Closed_Icon);
		private static Image deleteIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Button_Delete);
		private static Image dropArrowIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Button_Drop_Down);
		private static Image classIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Class_Type);
		private static Image arrayIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Array_Type);
		private static Image leftBracketIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Left_Button);
		private static Image rightBracketIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Right_Button);
		private static Image changeIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Exchange_Button);
		private static Image unsignedIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Unsigned_Type);
		private static Image signedIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Signed_Type);
		private static Image floatIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Float_Type);
		private static Image doubleIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Double_Type);
		private static Image vectorIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Vector_Type);
		private static Image matrixIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Matrix_Type);
		private static Image textIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Text_Type);
		private static Image pointerIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Pointer_Type);
		private static Image functionIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Function_Type);
		private static Image vtableIcon = DpiUtil.ScaleImage(Properties.Resources.B16x16_Interface_Type);

		public static Image OpenCloseOpen => openCloseOpenIcon;
		public static Image OpenCloseClosed => openCloseClosedIcon;
		public static Image Delete => deleteIcon;
		public static Image DropArrow => dropArrowIcon;
		public static Image Class => classIcon;
		public static Image Array => arrayIcon;
		public static Image LeftArrow => leftBracketIcon;
		public static Image RightArrow => rightBracketIcon;
		public static Image Change => changeIcon;
		public static Image Unsigned => unsignedIcon;
		public static Image Signed => signedIcon;
		public static Image Float => floatIcon;
		public static Image Double => doubleIcon;
		public static Image Vector => vectorIcon;
		public static Image Matrix => matrixIcon;
		public static Image Text => textIcon;
		public static Image Pointer => pointerIcon;
		public static Image Function => functionIcon;
		public static Image VTable => vtableIcon;
	}
}
