using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventSite.Domain.Infrastructure {
    public static class ResultStatus {
        public static string Success = "Success";
        public static string Failure = "Failure";
        public static string NotFound = "Not Found";
        public static string Forbidden = "Forbidden";
    }

    public static class ResultExtensions {
        public static bool Failed(this Result result) {
            return result.Status == ResultStatus.Failure;
        }

        public static bool Succeeded(this Result result) {
            return result.Status == ResultStatus.Success;
        }

        public static bool WasNotFound(this Result result) {
            return result.Status == ResultStatus.NotFound;
        }

        public static bool WasForbidden(this Result result) {
            return result.Status == ResultStatus.Forbidden;
        }
    }

    public class Result<T> : Result {
        public T Subject { get; private set; }

        public Result<T> WithSubject(T subject) {
            Subject = subject;
            return this;
        }

        public new Result<T> WithMessage(string message) {
            base.WithMessage(message);
            return this;
        }

        public new Result<T> WithErrorMessage(string message) {
            base.WithErrorMessage(message);
            return this;
        }

        public new Result<T> WithError(string property, string message) {
            base.WithError(property, message);
            return this;
        }

        public new Result<T> WithError(ValidationResult error) {
            base.WithError(error);
            return this;
        }

        public new Result<T> Subsume(Result result) {
            base.Subsume(result);
            return this;
        }
    }

    public class Result {
        const string DefaultFailureMessage = "The following problems were found:";

        public string Status { get; set; }
        public string Message { get; set; }
        public IList<Detail> Details { get; private set; }

        public Result() {
            Details = new List<Detail>();
            Status = ResultStatus.Success;
        }

        public Result WithMessage(string message) {
            Message = message;
            return this;
        }

        public Result WithErrorMessage(string message) {
            Message = message;
            Status = ResultStatus.Failure;
            return this;
        }

        public Result WithError(string property, string message) {
            if(string.IsNullOrEmpty(Message)) {
                Message = DefaultFailureMessage;
            }

            Status = ResultStatus.Failure;
            Details.Add(new Detail {
                Name = property,
                Description = message
            });

            return this;
        }

        public Result WithError(ValidationResult error) {
            return WithError(string.Join(", ", error.MemberNames), error.ErrorMessage);
        }

        public Result Subsume(Result subject) {
            if(subject.Status == ResultStatus.Failure) {
                Status = ResultStatus.Failure;
            }

            if(!string.IsNullOrEmpty(subject.Message)) {
                if(Message == null) {
                    Message = string.Empty;
                }

                Message += subject.Message;
            }

            subject.Details.Apply(x => Details.Add(x));

            if(string.IsNullOrEmpty(Message) && this.Failed()) {
                Message = DefaultFailureMessage;
            }

            return this;
        }

        public TResult To<TResult>() where TResult : Result, new() {
            var result = new TResult();
            result.Subsume(this);
            return result;
        }

        public static Result NotFound() {
            return new Result {Status = ResultStatus.NotFound};
        }

        public static Result Forbidden() {
            return new Result {Status = ResultStatus.Forbidden};
        }

        public static TResult NotFound<TResult>() where TResult : Result, new() {
            return new TResult {Status = ResultStatus.NotFound};
        }

        public static TResult Forbidden<TResult>() where TResult : Result, new() {
            return new TResult {Status = ResultStatus.Forbidden};
        }

        public static Result SuccessMessage(string message) {
            return new Result().WithMessage(message);
        }

        public static TResult ErrorMessage<TResult>(string message) where TResult : Result, new() {
            return ErrorMessage(message).To<TResult>();
        }

        public static Result ErrorMessage(string message) {
            return new Result().WithErrorMessage(message);
        }

        public static TResult Error<TResult>(string name, string description) where TResult : Result, new() {
            return Error(name, description).To<TResult>();
        }

        public static Result Error(string name, string description) {
            return new Result().WithError(name, description);
        }

        public static Result<T> Of<T>(T subject) {
            return new Result<T>().WithSubject(subject);
        }

        public class Detail {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}