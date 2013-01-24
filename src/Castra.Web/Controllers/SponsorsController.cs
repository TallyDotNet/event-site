namespace Castra.Web.Controllers
{
	using System.Web.Mvc;
    using BlueSpire.Kernel.Bus;
    using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using Queries;

	public class SponsorsController : ControllerBase
    {
        public SponsorsController(IApplicationBus<IContextProvider> bus)
            : base(bus)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
        	var sponsors = Bus.Get(new Sponsors());

			return View(sponsors);
        }

		[ChildActionOnly]
		public ActionResult Featured()
		{
			var sponsor = Bus.Get(new RandomFeaturedSponsor());
			return (sponsor == null)
			 ? (ActionResult) new EmptyResult() 
			 : View("Featured", "None", sponsor);
		}
    }
}