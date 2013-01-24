namespace Castra.Web.Controllers.Scaffold
{
	using BlueSpire.Web.Mvc.Scaffold;
	using Models;
	using NHibernate;

	public class AdminConferenceController : ScaffoldController<Conference>
	{
		public AdminConferenceController(ISession session)
			: base(session)
		{
		}
	}
}