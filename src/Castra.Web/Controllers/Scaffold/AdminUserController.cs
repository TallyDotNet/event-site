namespace Castra.Web.Controllers.Scaffold
{
	using BlueSpire.Web.Mvc.Membership;
	using BlueSpire.Web.Mvc.Scaffold;
	using Models;
	using NHibernate;

	public class AdminUserController : ScaffoldController<User>
	{
		public AdminUserController(ISession session)
			: base(session)
		{
		}
	}
}