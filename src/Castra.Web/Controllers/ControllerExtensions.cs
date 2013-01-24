namespace Castra.Web.Controllers
{
    using System.Web.Mvc;
    using BlueSpire.Kernel.Bus;
    using Caliburn.Core;

    public static class ControllerExtensions
    {
        public static void AddErrorsFrom(this ModelStateDictionary state, Result result)
        {
            result.Errors.Apply(x => state.AddModelError(x.Key, x.Message));
        }
    }
}