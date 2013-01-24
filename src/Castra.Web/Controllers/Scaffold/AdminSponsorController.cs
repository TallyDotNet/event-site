namespace Castra.Web.Controllers.Scaffold
{
	using BlueSpire.Web.Mvc.Scaffold;
	using Models;
    using NHibernate;

    public class AdminSponsorController : ScaffoldController<Sponsor>
    {
        public AdminSponsorController(ISession session)
            : base(session)
        {
        }
    }
}