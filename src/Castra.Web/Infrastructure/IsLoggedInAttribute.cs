namespace Castra.Web.Infrastructure
{
	using System.Web.Mvc;
	using System.Web.Routing;
	using BlueSpire.Web.Mvc.Membership;

	public class IsLoggedInAttribute : IsLoggedInAttributeBase
	{
		public override void WhenNotLoggedIn(ActionExecutingContext filterContext)
		{
			string url = filterContext.HttpContext.Request.RawUrl;
			filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
			                                                 	{
			                                                 		{"controller", "Account"},
			                                                 		{"action", "Login"},
			                                                 		{"returnUrl", url},
			                                                 	});
		}
	}
}