using System.Web.Mvc;
using CodeCamp.Domain.Infrastructure;
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

        protected CommandExecutor<TResult> Execute<TResult>(ICommand<TResult> command)
            where TResult : Result, new() {
            return new CommandExecutor<TResult>(this, command);
        }
    }
}