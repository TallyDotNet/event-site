namespace Castra.Web.Queries
{
	using System.Linq;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class SpeakersHandler : PageQueryHandler<Speakers, Profile>
	{
		public SpeakersHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		protected override IQueryable<Profile> CreateQuery(Speakers query, IQueryable<Profile> speakers)
		{
			return from speaker in speakers
				   where speaker.HasApprovedSessions
				   select speaker;
		}
	}
}