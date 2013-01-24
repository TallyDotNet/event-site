namespace Castra.Web.Queries
{
	using BlueSpire.Kernel.Data;
	using Models;

	public class Speakers : IPagedQuery<Profile>
	{
		public Speakers(int page)
		{
			PageNumber = page;
			PageSize = 50;
		}

		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }
	}
}