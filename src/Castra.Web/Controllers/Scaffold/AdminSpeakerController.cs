namespace Castra.Web.Controllers.Scaffold
{
	using BlueSpire.Web.Mvc.Scaffold;
	using Models;
	using NHibernate;

	public class AdminSpeakerController : ScaffoldController<Profile>
	{
		public AdminSpeakerController(ISession session)
			: base(session)
		{
		}
	}
}