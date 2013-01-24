namespace Castra.Web.Queries
{
	using BlueSpire.Kernel.Data;
	using Models;

	public class Sponsors : IPagedQuery<Sponsor>
	{
		public int PageNumber
		{
			get { return 1; }
		}

		public int PageSize
		{
			get { return 100; }
		}
	}
}