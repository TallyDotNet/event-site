namespace Castra.Web.Queries
{
	using System.Linq;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class SpeakerByLookupNameHandler : ValueQueryHandler<SpeakerByLookupName, Profile>
	{
		public SpeakerByLookupNameHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		protected override IQueryable<Profile> CreateQuery(SpeakerByLookupName query, IQueryable<Profile> speakers)
		{
			return from speaker in speakers
				   where speaker.LookupName == query.LookupName
				   select speaker;
		}
	}
}