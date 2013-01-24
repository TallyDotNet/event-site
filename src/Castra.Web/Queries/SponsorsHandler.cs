namespace Castra.Web.Queries
{
	using System.Linq;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class SponsorsHandler : PageQueryHandler<Sponsors, Sponsor>
	{
		public SponsorsHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		protected override IQueryable<Sponsor> CreateQuery(Sponsors query, IQueryable<Sponsor> sponsors)
		{
			return from sponsor in sponsors
			       where sponsor.IsActive
				   orderby sponsor.SortOrder 
				   orderby sponsor.Name
			       select sponsor;
		}
	}
}