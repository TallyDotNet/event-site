using System.Web.Mvc;
using CodeCamp.Infrastructure.Models;

namespace CodeCamp.Infrastructure.Controllers {
    public class BaseController : Controller {
        public PageInfo Info { get; private set; }

        protected BaseController() {
            Info = DependencyResolver.Current.GetService<PageInfo>();
        }
    }
}