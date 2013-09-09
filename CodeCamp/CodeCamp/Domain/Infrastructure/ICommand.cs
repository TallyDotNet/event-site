using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NLog;
using Raven.Client;

namespace CodeCamp.Domain.Infrastructure {
    public interface ICommand<out TResponse> where TResponse : Result {
        TResponse Process();
    }

    public abstract class Command<TResponse> : ICommand<TResponse> where TResponse : Result, new() {
        protected Logger Log { get; set; }
        public IApplicationBus Bus { get; set; }
        public IApplicationState State { get; set; }
        public IDocumentSession DocSession { get; set; }

        protected Command() {
            Log = LogManager.GetLogger(GetType().FullName);
        }

        protected abstract TResponse Execute();

        public TResponse Process() {
            try {
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
    }
}