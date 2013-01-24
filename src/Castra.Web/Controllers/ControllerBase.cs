namespace Castra.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using BlueSpire.Kernel.Bus;
    using BlueSpire.Web.Mvc.Infrastructure;
    using Models;

    public abstract class ControllerBase : Controller
    {
        protected readonly IApplicationBus<IContextProvider> Bus;

        protected ControllerBase(IApplicationBus<IContextProvider> bus)
        {
            Bus = bus;
        }

        protected override void HandleUnknownAction(string actionName)
        {
            try
            {
                View(actionName).ExecuteResult(ControllerContext);
            }
            catch (InvalidOperationException ieox)
            {
                ViewData["error"] = "Unknown Action: \"" + Server.HtmlEncode(actionName) + "\"";
                ViewData["exMessage"] = ieox.Message;
                View("Error").ExecuteResult(ControllerContext);
            }
        }

        protected ActionResult Post<TResult>(ICommand<TResult> command, Func<TResult, ActionResult> onSuccess)
            where TResult : Result
        {
            if (!ModelState.IsValid)
                return View(command);

            var result = Bus.Post(command);

            if (result.Failed())
            {
                ModelState.AddErrorsFrom(result);
                return View(command);
            }

            return onSuccess(result);
        }
    }
}