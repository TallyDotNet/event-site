using System.Web.Mvc;
using CodeCamp.Infrastructure.Models;
using NLog;

namespace CodeCamp.Infrastructure.Controllers {
    public class BaseController : Controller {
        public PageInfo Info { get; private set; }
        public Logger Log { get; set; }

        protected BaseController() {
            Info = DependencyResolver.Current.GetService<PageInfo>();
        }
    }
}