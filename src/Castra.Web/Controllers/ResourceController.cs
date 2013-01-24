namespace Castra.Web.Controllers
{
	using System;
	using System.Web.Mvc;
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using Models;

	public class ResourceController : Controller
	{
		protected readonly IApplicationBus<IContextProvider> Bus;

		public ResourceController(IApplicationBus<IContextProvider> bus)
		{
			Bus = bus;
		}

		[HttpGet]
		public ActionResult Index(Guid id)
		{
			var resource = Bus.Get<Blob>(id);

			return new FileContentResult(resource.Data, resource.ContentType.DisplayName);
		}
	}
}