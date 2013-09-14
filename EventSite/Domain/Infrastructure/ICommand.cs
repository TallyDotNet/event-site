using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventSite.Domain.Model;
using NLog;
using Raven.Client;

namespace EventSite.Domain.Infrastructure {
    public interface ICommand<out TResponse> where TResponse : Result {
        TResponse Process();
    }

    public abstract class Command<TResponse> : ICommand<TResponse> where TResponse : Result, new() {
        string restrictedRole;

        protected Logger Log { get; set; }
        public IApplicationBus Bus { get; set; }
        public IApplicationState State { get; set; }
        public IDocumentSession DocSession { get; set; }

        protected User CurrentUser {
            get { return State.User; }
        }

        protected Command() {
            Log = LogManager.GetLogger(GetType().FullName);
        }

        protected abstract TResponse Execute();

        public TResponse Process() {
            try {
                if(!string.IsNullOrEmpty(restrictedRole)) {
                    if(!State.UserIsLoggedIn() || !CurrentUser.InRole(restrictedRole)) {
                        return Forbidden();
                    }
                }

                var initialCheck = Validate(this);
                return initialCheck.Failed() ? initialCheck : Execute();
            } catch(Exception e) {
                Log.Error(e);
                var result = new TResponse();
                result.WithErrorMessage("An unexpected error occurred. Please try again later.");
                return result;
            }
        }

        protected TResponse NotFound() {
            return Result.NotFound<TResponse>();
        }

        protected TResponse Forbidden() {
            return Result.Forbidden<TResponse>();
        }

        protected TResponse PropertyError(string property, string message) {
            var result = new TResponse();
            result.WithError(property, message);
            return result;
        }

        protected TResponse Error(string message) {
            var result = new TResponse();
            result.WithErrorMessage(message);
            return result;
        }

        protected TResponse ErrorFormat(string messageFormat, params object[] args) {
            var result = new TResponse();
            result.WithErrorMessage(string.Format(messageFormat, args));
            return result;
        }

        protected TResponse Success(string messageFormat) {
            var result = new TResponse();
            result.WithMessage(messageFormat);
            return result;
        }

        protected TResponse SuccessFormat(string messageFormat, params object[] args) {
            var result = new TResponse();
            result.WithMessage(string.Format(messageFormat, args));
            return result;
        }

        protected DateTimeOffset Now() {
            return DateTimeOffset.Now;
        }

        protected void RestrictToRole(string role) {
            restrictedRole = role;
        }

        TResponse Validate(object instance) {
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(
                instance,
                new ValidationContext(instance, null, new Dictionary<object, object>()),
                results
                );

            var result = new TResponse();
            results.Apply(x => result.WithError(x));

            return result;
        }

        public abstract class AdminOnly : Command<TResponse> {
            protected AdminOnly() {
                RestrictToRole(Roles.Admin);
            }
        }
    }
}