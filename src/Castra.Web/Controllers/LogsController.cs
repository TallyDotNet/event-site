namespace Castra.Web.Controllers
{
	using System;
	using System.IO;
	using System.Web.Mvc;
	using BlueSpire.Web.Mvc.Membership;

	public class LogsController : Controller
	{
		[HttpGet]
		[IsAdmin]
		public ActionResult Index()
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", "log.txt");
			return new FilePathResult(path, "text/plain");
		}
	}
}