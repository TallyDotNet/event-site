namespace Castra.Web.Queries
{
	using BlueSpire.Kernel.Data;
	using Models;

	public class Sessions : IPagedQuery<Session>
	{
		public Sessions(int page)
		{
			PageNumber = page;
			PageSize = 50;
		}

		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }
		public string GroupBy { get; set; }
	}
}