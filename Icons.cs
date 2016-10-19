using System.Drawing;

namespace ReClassNET
{
	class Icons
	{
		private static Image openCloseOpenIcon = DpiUtil.ScaleImage(Properties.Resources.open_icon);
		private static Image openCloseClosedIcon = DpiUtil.ScaleImage(Properties.Resources.closed_icon);
		private static Image deleteIcon = DpiUtil.ScaleImage(Properties.Resources.cross);
		private static Image dropArrowIcon = DpiUtil.ScaleImage(Properties.Resources.arrow_down_icon);
		private static Image classIcon = DpiUtil.ScaleImage(Properties.Resources.class_icon);
		private static Image arrayIcon = DpiUtil.ScaleImage(Properties.Resources.array_icon);
		private static Image leftBracketIcon = DpiUtil.ScaleImage(Properties.Resources.left_icon);
		private static Image rightBracketIcon = DpiUtil.ScaleImage(Properties.Resources.right_icon);
		private static Image changeIcon = DpiUtil.ScaleImage(Properties.Resources.exchange_icon);
		private static Image unsignedIcon = DpiUtil.ScaleImage(Properties.Resources.unsigned_icon);
		private static Image signedIcon = DpiUtil.ScaleImage(Properties.Resources.signed_icon);
		private static Image floatIcon = DpiUtil.ScaleImage(Properties.Resources.float_icon);
		private static Image doubleIcon = DpiUtil.ScaleImage(Properties.Resources.double_icon);
		private static Image vectorIcon = DpiUtil.ScaleImage(Properties.Resources.vector_icon);
		private static Image matrixIcon = DpiUtil.ScaleImage(Properties.Resources.matrix_icon);
		private static Image textIcon = DpiUtil.ScaleImage(Properties.Resources.text_icon);
		private static Image pointerIcon = DpiUtil.ScaleImage(Properties.Resources.pointer_icon);
		private static Image functionIcon = DpiUtil.ScaleImage(Properties.Resources.function_icon);
		private static Image vtableIcon = DpiUtil.ScaleImage(Properties.Resources.interface_icon);

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
