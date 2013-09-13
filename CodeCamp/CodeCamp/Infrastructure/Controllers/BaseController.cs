using System.Web.Mvc;
using CodeCamp.Domain.Infrastructure;
using Raven.Client;
using Raven.Imports.Newtonsoft.Json;

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

        public Result Result {
            get {
                var message = TempData["Result"];
                if(message != null) {
                    return JsonConvert.DeserializeObject<Result>(message.ToString());
                }

                return null;
            }
            set {
                TempData["Result"] = JsonConvert.SerializeObject(value);
            }
        }

        protected void DisplayErrorMessage(string message) {
            Result = Result.ErrorMessage(message);
        }

        protected CommandExecutor<TResult> Execute<TResult>(ICommand<TResult> command)
            where TResult : Result, new() {
            return new CommandExecutor<TResult>(this, command);
        }
    }
}