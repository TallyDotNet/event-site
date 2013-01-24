namespace Castra.Web.Controllers
{
	using System.Web.Mvc;

	public class ErrorController : Controller
	{
		public ActionResult Unknown()
		{
			return View();
		}

		public ActionResult NotAuthorized()
		{
			return View();
		}
	}
}