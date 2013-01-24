namespace Castra.Web.Queries
{
	using System.Linq;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class ProposalsBySpeakerIdHandler : PageQueryHandler<ProposalsBySpeakerId, SessionProposal>
	{
		public ProposalsBySpeakerIdHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		protected override IQueryable<SessionProposal> CreateQuery(ProposalsBySpeakerId query, IQueryable<SessionProposal> proposals)
		{
			return from proposal in proposals
				   where proposal.Speaker.Id == query.SpeakerId
				   select proposal;
		}
	}
}