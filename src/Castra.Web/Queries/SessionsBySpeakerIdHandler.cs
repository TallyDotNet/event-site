namespace Castra.Web.Queries
{
	using System.Linq;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class SessionsBySpeakerIdHandler : PageQueryHandler<SessionsBySpeakerId, Session>
	{
		public SessionsBySpeakerIdHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		protected override IQueryable<Session> CreateQuery(SessionsBySpeakerId query, IQueryable<Session> sessions)
		{
			return from session in sessions
				   where session.Speaker.Id == query.SpeakerId
				   select session;
		}
	}
}