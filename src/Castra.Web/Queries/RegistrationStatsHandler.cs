namespace Castra.Web.Queries
{
	using System.Collections.Generic;
	using BlueSpire.Kernel;
	using BlueSpire.NHibernate;
	using BlueSpire.NHibernate.QueryHandlers;

	public class RegistrationStatsHandler : QueryHandler<RegistrationStats>
	{
		public RegistrationStatsHandler(ISessionSource sessionSource) : base(sessionSource)
		{
		}

		public override IFuture Handle(RegistrationStats query)
		{
			var attendees = Session.CreateQuery("select count(*) from Profile").FutureValue<long>();
			var speakers = Session.CreateQuery("select count(*) from Profile where HasApprovedSessions = true").FutureValue<long>();
			var sessions = Session.CreateQuery("select count(*) from Session").FutureValue<long>();

			return Future.Of(() => new Dictionary<string, long>
			                       	{
			                       		{"Attendees", attendees.Value},
										{"Speakers", speakers.Value},
										{"Sessions", sessions.Value},
			                       	});
		}
	}
}