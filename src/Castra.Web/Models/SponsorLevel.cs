namespace Castra.Web.Models
{
	using System;
	using BlueSpire.Kernel;

	[Serializable]
	public class SponsorLevel : Enumeration
	{
		public static SponsorLevel Basic = new SponsorLevel(3, "Platinum");
		public static SponsorLevel Garnet = new SponsorLevel(1, "Garnet");
		public static SponsorLevel Gold = new SponsorLevel(2, "Gold");

		public SponsorLevel(int value, string displayName)
			: base(value, displayName)
		{
		}
	}
}