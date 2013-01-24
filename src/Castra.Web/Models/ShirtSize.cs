namespace Castra.Web.Models
{
	using System;
	using BlueSpire.Kernel;

	[Serializable]
	public class ShirtSize : Enumeration
	{
		public static ShirtSize S = new ShirtSize(0, "S");
		public static ShirtSize M = new ShirtSize(1, "M");
		public static ShirtSize L = new ShirtSize(3, "L");
		public static ShirtSize XL = new ShirtSize(4, "XL");
		public static ShirtSize XXL = new ShirtSize(5, "2XL");
		public static ShirtSize XXXL = new ShirtSize(6, "3XL");

		public ShirtSize(int value, string displayName)
			: base(value, displayName)
		{
		}
	}
}