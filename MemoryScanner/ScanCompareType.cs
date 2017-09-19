using System.ComponentModel;

namespace ReClassNET.MemoryScanner
{
	public enum ScanCompareType
	{
		[Description("Is Equal")]
		Equal,
		[Description("Is Not Equal")]
		NotEqual,
		[Description("Has Changed")]
		Changed,
		[Description("Has Not Changed")]
		NotChanged,
		[Description("Is Greater Than")]
		GreaterThan,
		[Description("Is Greater Than Or Equal")]
		GreaterThanOrEqual,
		[Description("Has Increased")]
		Increased,
		[Description("Has Increased Or Is Equal")]
		IncreasedOrEqual,
		[Description("Is Less Than")]
		LessThan,
		[Description("Is Less Than Or Equal")]
		LessThanOrEqual,
		[Description("Has Decreased")]
		Decreased,
		[Description("Has Decreased Or Is Equal")]
		DecreasedOrEqual,
		[Description("Is Between")]
		Between,
		[Description("Is Between Or Equal")]
		BetweenOrEqual,
		[Description("Unknown Initial Value")]
		Unknown
	}
}
