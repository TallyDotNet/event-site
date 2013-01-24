namespace Castra.Web.Controllers
{
	using System;
	using System.Web.Mvc;
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using Models;
	using Queries;

	public class SpeakersController : ControllerBase
	{
		public SpeakersController(IApplicationBus<IContextProvider> bus)
			: base(bus)
		{
		}

		[HttpGet]
		public ActionResult Index(int? page)
		{
			var speakers = Bus.Get(new Speakers(page ?? 1));
			return View(speakers);
		}

		[HttpGet]
		public ActionResult Details(string lookupName)
		{
			var speaker = Bus.Get(new SpeakerByLookupName(lookupName)) ?? new Profile();
			return View(speaker);
		}

		[ChildActionOnly]
		public ActionResult Proposals(Guid speakerId)
		{
			var proposals = Bus.Get(new ProposalsBySpeakerId(speakerId));
			return View("Proposals", "None", proposals);
		}

		[ChildActionOnly]
		public ActionResult Sessions(Guid speakerId)
		{
			var sessions = Bus.Get(new SessionsBySpeakerId(speakerId));
			return View("Sessions", "None", sessions);
		}
	}
}