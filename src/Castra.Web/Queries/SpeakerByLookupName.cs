namespace Castra.Web.Queries
{
	using BlueSpire.Kernel.Data;
	using Models;

	public class SpeakerByLookupName : IQuery<Profile>
	{
		public string LookupName { get; private set; }

		public SpeakerByLookupName(string lookupName)
		{
			LookupName = lookupName;
		}
	}
}