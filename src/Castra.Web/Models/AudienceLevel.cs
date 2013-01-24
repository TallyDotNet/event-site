namespace Castra.Web.Models
{
	using System;
	using BlueSpire.Kernel;

	[Serializable]
	public class AudienceLevel : Enumeration
	{
		public static AudienceLevel General = new AudienceLevel(0, "General");
		public static AudienceLevel Beginner = new AudienceLevel(1000, "Beginner");
		public static AudienceLevel Intermediate = new AudienceLevel(2000, "Intermediate");
		public static AudienceLevel Advanced = new AudienceLevel(3000, "Advanced");

		public AudienceLevel(int value, string displayName)
			: base(value, displayName)
		{
		}
	}
}