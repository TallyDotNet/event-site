namespace Castra.Web.Queries
{
	using BlueSpire.Kernel.Data;
	using Models;

	public class Proposals : IPagedQuery<SessionProposal>
	{
		public Proposals(int page)
		{
			PageNumber = page;
			PageSize = 25;
		}

		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }
	}
}