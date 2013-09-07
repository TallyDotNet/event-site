using System.Web.Mvc;
using Raven.Client;

namespace CodeCamp.Infrastructure.Controllers {
    public abstract class BaseController : LowLevelController {
        protected IDocumentSession DocSession { get; private set; }
        internal bool ErrorOccurred = false;

        protected BaseController() {
            DocSession = DependencyResolver.Current.GetService<IDocumentSession>();
        }

        protected override void OnActionExecuted(ActionExecutedContext aec) {
            if(aec.IsChildAction) {
                return;
            }

            try {
                if(aec.Exception != null || ErrorOccurred) {
                    return;
                }

                DocSession.SaveChanges();
            } finally {
                DocSession.Dispose();
            }
        }
    }
}