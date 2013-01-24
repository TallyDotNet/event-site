namespace Castra.Web.Queries
{
	using BlueSpire.Kernel.Data;
	using Models;

	public class SessionByLookupName : IQuery<Session>
	{
		public string LookupName { get; private set; }

		public SessionByLookupName(string lookupName)
		{
			LookupName = lookupName;
		}
	}
}