namespace Castra.Web.Controllers
{
	using System.Web.Mvc;
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using Commands;
	using Infrastructure;
	using Models;
	using Queries;

	public class RegistrationController : ControllerBase
	{
		public RegistrationController(IApplicationBus<IContextProvider> bus) : base(bus)
		{
		}

		[HttpPost]
		public ActionResult Attend(CreateUser form)
		{
			return Post(form, result => RedirectToAction("Profile"));
		}

		[ChildActionOnly]
		public ActionResult Stats()
		{
			var stats = Bus.Get(new RegistrationStats());
			return View("Stats", "None", stats);
		}

		[HttpGet]
		[IsLoggedIn]
		public ActionResult Edit()
		{
			ViewData["UserID"] = Bus.Context.User.Id;

			var profile = Bus.Get(new QueryById<Profile> {Id = Bus.Context.User.Id});
			var form = AccountController.CreateFormFrom(profile);

			return View(form);
		}

		[HttpPost]
		[IsLoggedIn]
		public ActionResult Edit(EditProfile form)
		{
			return Post(form, result => RedirectToAction("EditProfile"));
		}

		[HttpGet]
		[IsLoggedIn]
		public ActionResult Profile()
		{
			var profile = Bus.Get(new QueryById<Profile> {Id = Bus.Context.User.Id});
			var form = AccountController.CreateFormFrom(profile);

			return View(form);
		}

		[HttpPost]
		[IsLoggedIn]
		public ActionResult Profile(EditProfile form)
		{
			return Post(form, result => RedirectToAction("Thanks"));
		}
	}
}