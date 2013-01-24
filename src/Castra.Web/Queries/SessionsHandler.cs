namespace Castra.Web.Queries
{
	using System.Linq;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class SessionsHandler : PageQueryHandler<Sessions, Session>
	{
		public SessionsHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		protected override IQueryable<Session> CreateQuery(Sessions query, IQueryable<Session> sessions)
		{
			return from session in sessions
			       orderby session.Title
			       select session;
		}
	}
}