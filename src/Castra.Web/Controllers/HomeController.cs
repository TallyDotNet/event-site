namespace Castra.Web.Controllers
{
    using System.Web.Mvc;
    using BlueSpire.Kernel.Bus;
    using BlueSpire.Web.Mvc.Infrastructure;
    using BlueSpire.Web.Mvc.Membership;

	public class HomeController : ControllerBase
    {
        private readonly MainMenu _mainMenu;

        public HomeController(IApplicationBus<IContextProvider> bus, MainMenu mainMenu)
            : base(bus)
        {
            _mainMenu = mainMenu;
        }

		[IsAdmin]
		public ActionResult Admin()
		{
			return View();
		}

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Location()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            //HACK: Not sure why, but this always uses a master page. I provide an empty one, otherwise it uses _Application. This be nhaml's fault.
            return View("Menu", "None", _mainMenu);
        }
    }
}