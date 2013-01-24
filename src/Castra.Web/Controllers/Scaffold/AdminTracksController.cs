namespace Castra.Web.Controllers.Scaffold
{
	using BlueSpire.Web.Mvc.Scaffold;
	using Models;
	using NHibernate;

	public class AdminTracksController : ScaffoldController<Track>
	{
		public AdminTracksController(ISession session)
			: base(session)
		{
		}
	}
}