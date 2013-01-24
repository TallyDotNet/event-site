namespace Castra.Web.Queries
{
	using BlueSpire.Kernel;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;
	using Models;

	public class RandomFeaturedSponsorHandler : QueryHandler<RandomFeaturedSponsor>
	{
		public RandomFeaturedSponsorHandler(ISessionSource sessionSource)
			: base(sessionSource)
		{
		}

		public override IFuture Handle(RandomFeaturedSponsor query)
		{
			var q = Session.CreateQuery("from Sponsor where IsActive = 1 order by newid()").SetMaxResults(1);

			var futureValue = q.FutureValue<Sponsor>();
			return Future.Of(() => futureValue.Value);
		}
	}
	}