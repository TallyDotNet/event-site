using System;
using System.Web.Mvc;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Infrastructure.Results;
using Raven.Imports.Newtonsoft.Json;

namespace CodeCamp.Infrastructure.Controllers {
    public class CommandExecutor<TResult>
        where TResult : Result, new() {
        readonly BaseController controller;
        readonly ICommand<TResult> command;
        Func<TResult, ActionResult> onFailure;
        Func<TResult, ActionResult> onSuccess;
        bool reportError = true;
        bool reportSuccess = true;

        public CommandExecutor(BaseController controller, ICommand<TResult> command) {
            this.controller = controller;
            this.command = command;
        }

        public CommandExecutor<TResult> OnSuccess(Func<TResult, ActionResult> onSuccess) {
            this.onSuccess = onSuccess;
            return this;
        }

        public CommandExecutor<TResult> OnFailure(Func<TResult, ActionResult> onFailure) {
            this.onFailure = onFailure;
            return this;
        }

        public CommandExecutor<TResult> Always(Func<TResult, ActionResult> always) {
            onFailure = always;
            onSuccess = always;
            return this;
        }

        public CommandExecutor<TResult> ReportErrorMessage(bool report) {
            reportError = report;
            return this;
        }

        public CommandExecutor<TResult> ReportSuccessMessage(bool report) {
            reportSuccess = report;
            return this;
        }

        public CommandExecutor<TResult> ReportMessages(bool report) {
            reportSuccess = report;
            reportError = report;
            return this;
        }

        public static implicit operator ActionResult(CommandExecutor<TResult> executor) {
            var result = executor.controller.Bus.Execute(executor.command);

            if(result.WasNotFound()) {
                return executor.controller.NotFound();
            }

            if(result.WasForbidden()) {
                return executor.controller.Forbidden();
            }

            if(result.Failed()) {
                executor.controller.ErrorOccurred = true;

                if(executor.controller.Request.IsAjaxRequest() && executor.onFailure == null) {
                    executor.controller.Response.StatusCode = 500;
                    return new JsonNetResult(result);
                }

                if(executor.onFailure != null) {
                    if(executor.reportError) {
                        executor.controller.TempData["Result"] = JsonConvert.SerializeObject(result);
                    }

                    return executor.onFailure(result);
                }

                return executor.controller.CreateErrorActionResult();
            }

            if(executor.controller.Request.IsAjaxRequest() && executor.onSuccess == null) {
                return new JsonNetResult(result);
            }

            if(executor.reportSuccess) {
                executor.controller.TempData["Result"] = JsonConvert.SerializeObject(result);
            }

            if(executor.onSuccess != null) {
                return executor.onSuccess(result);
            }

            return new ViewResult();
        }

        public Result Response { get; private set; }
    }
}