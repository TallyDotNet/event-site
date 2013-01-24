namespace Castra.Web.Controllers
{
	using System;
	using System.Linq;
	using System.Web.Mvc;
	using BlueSpire.Kernel;
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using BlueSpire.Web.Mvc.Membership;
	using Commands;
	using Infrastructure;
	using Models;
	using Queries;

	public class SessionController : ControllerBase
	{
		public SessionController(IApplicationBus<IContextProvider> bus)
			: base(bus)
		{
		}


		[HttpGet]
		public ActionResult Index(int? page, string groupBy)
		{
			var sessions = Bus.Get(new Sessions(page ?? 1) {GroupBy = groupBy});

			Func<Session, string> grouper;
			Func<string, string> sorter = x=>x;

			switch ((groupBy ?? "level").ToLowerInvariant())
			{
				case "track":
					grouper = x => (x.Track != null) ? x.Track.Title : "TBD";
					break;

				case "room":
					grouper = x => x.Room ?? "TDB";
					break;

				default:
					grouper = x => x.AudienceLevel.DisplayName;
					sorter = x => Enumeration.FromDisplayName(x, AudienceLevel.General).Value.ToString();
					break;
			}

			var vm = from session in sessions
			         orderby session.Title
			         group session by grouper(session)
			         into g
					 orderby sorter(g.Key)
			         select new {g.Key, Items = g};

			return View(vm.ToDictionary(x => x.Key, x => x.Items));
		}

		[HttpGet]
		public ActionResult Details(string lookupName)
		{
			var session = Bus.Get(new SessionByLookupName(lookupName)) ?? new Session();
			return View(session);
		}

		[HttpGet]
		[IsAdmin]
		public ActionResult Proposals()
		{
			var proposals = Bus.Get(new Proposals(1));
			return View(proposals);
		}

		[IsAdmin]
		public ActionResult Approve(Guid proposalId)
		{
			return Post(new ApproveProposal(proposalId), result => RedirectToAction("Proposals"));
		}

		[HttpGet]
		[IsLoggedIn]
		public ActionResult Propose()
		{
			return View(new ProposeSession());
		}

		[HttpPost]
		[IsLoggedIn]
		public ActionResult Propose(ProposeSession form)
		{
			return Post(form, result => RedirectToAction("EditProfile", "Account"));
		}

		public ActionResult Edit()
		{
			return View();
		}
	}
}