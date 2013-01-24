namespace Castra.Web.Queries
{
	using System.Linq;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class SessionByLookupNameHandler : ValueQueryHandler<SessionByLookupName, Session>
	{
		public SessionByLookupNameHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		protected override IQueryable<Session> CreateQuery(SessionByLookupName query, IQueryable<Session> sessions)
		{
			return from session in sessions
			       where session.LookupName == query.LookupName
			       select session;
		}
	}
}