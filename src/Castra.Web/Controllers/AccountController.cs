namespace Castra.Web.Controllers
{
	using System.Web.Mvc;
	using BlueSpire.Kernel;
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using Commands;
	using Infrastructure;
	using Models;
	using Queries;

	public class AccountController : ControllerBase
	{
		public AccountController(IApplicationBus<IContextProvider> bus)
			: base(bus)
		{
		}

		[IsLoggedIn]
		public ActionResult Logout()
		{
			Bus.Context.ClearUser();
			return RedirectToAction("Index", "Home");
		}

		public ActionResult Status()
		{
			return View("Status", "None", Bus.Context.User);
		}

		public ActionResult Login(string returnUrl)
		{
			return View(new Login {ReturnUrl = returnUrl});
		}

		[HttpPost]
		public ActionResult Login(Login form)
		{
			return Post(form, result => string.IsNullOrEmpty(form.ReturnUrl)
			                            	? (ActionResult) RedirectToAction("EditProfile", "Account")
			                            	: Redirect(form.ReturnUrl));
		}

		public ActionResult New()
		{
			if(Bus.Context.IsLoggedIn()) return RedirectToAction("EditProfile", "Account");
			return View();
		}

		[HttpPost]
		public ActionResult New(CreateUser form)
		{
			return Post(form, result => RedirectToAction("SetupProfile"));
		}

		[HttpPost]
		public ActionResult Password(ChangePassword form)
		{
			return Post(form, result => RedirectToAction("SetupProfile"));
		}

		[HttpGet]
		public ActionResult IsAccountNameUnique(string username)
		{
			var isUnique = Bus.Get(new IsUsernameAvailable(username));
			return Json(isUnique);
		}

		[HttpGet]
		[IsLoggedIn]
		public ActionResult EditProfile()
		{
			ViewData["UserID"] = Bus.Context.User.Id;

			var profile = Bus.Get(new QueryById<Profile> {Id = Bus.Context.User.Id});
			var form = CreateFormFrom(profile);

			return View(form);
		}

		[HttpPost]
		[IsLoggedIn]
		public ActionResult EditProfile(EditProfile form)
		{
			//ViewData["Message"] = "Profile Updated.";
			return Post(form, result => RedirectToAction("EditProfile"));
		}

		[HttpGet]
		[IsLoggedIn]
		public ActionResult SetupProfile()
		{
			var profile = Bus.Get(new QueryById<Profile> {Id = Bus.Context.User.Id});
			var form = CreateFormFrom(profile);

			return View(form);
		}

		[HttpPost]
		[IsLoggedIn]
		public ActionResult SetupProfile(EditProfile form)
		{
			return Post(form, result => RedirectToAction("Propose", "Session"));
		}

		public static EditProfile CreateFormFrom(Profile profile)
		{
			var form = new EditProfile();
			if (profile.Found())
			{
				form.Bio = profile.Bio;
				form.BlogUrl = profile.BlogUrl;
				form.CompanyName = profile.CompanyName;
				form.CompanyUrl = profile.CompanyUrl;
				form.IsMVP = profile.IsMVP;
				form.Location = profile.Location;
				form.Name = profile.Name;
				form.Phone = profile.Phone;
				form.ShirtSize = profile.ShirtSize;
				form.Title = profile.Title;
				form.Twitter = profile.Twitter;
				form.IsPublic = profile.IsPublic;
				form.OptOut = profile.OptOut;
			}
			return form;
		}
	}
}