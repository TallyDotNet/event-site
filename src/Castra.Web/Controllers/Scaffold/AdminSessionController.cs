namespace Castra.Web.Controllers.Scaffold
{
	using BlueSpire.Web.Mvc.Scaffold;
	using Models;
    using NHibernate;

    public class AdminSessionController : ScaffoldController<Session>
    {
        public AdminSessionController(ISession session)
            : base(session)
        {
        }
    }
}