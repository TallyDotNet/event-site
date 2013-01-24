namespace Castra.Web.Controllers.Scaffold
{
	using BlueSpire.Web.Mvc.Scaffold;
	using Models;
	using NHibernate;

	public class AdminProposalController : ScaffoldController<SessionProposal>
	{
		public AdminProposalController(ISession session)
			: base(session)
		{
		}
	}
}