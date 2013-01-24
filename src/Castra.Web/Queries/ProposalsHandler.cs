namespace Castra.Web.Queries
{
	using System.Linq;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class ProposalsHandler : PageQueryHandler<Proposals, SessionProposal>
	{
		public ProposalsHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		protected override IQueryable<SessionProposal> CreateQuery(Proposals query, IQueryable<SessionProposal> proposals)
		{
			return from proposal in proposals
			       select proposal;
		}
	}
}