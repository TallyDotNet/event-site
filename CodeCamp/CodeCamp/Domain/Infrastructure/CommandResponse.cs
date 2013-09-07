using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeCamp.Domain.Infrastructure {
    public static class CommandResponseStatus {
        public static string Success = "Success";
        public static string Failure = "Failure";
        public static string NotFound = "Not Found";
        public static string Forbidden = "Forbidden";
    }

    public static class CommandResponseExtensions {
        public static bool Failed(this CommandResponse result) {
            return result.Status == CommandResponseStatus.Failure;
        }

        public static bool Succeeded(this CommandResponse result) {
            return result.Status == CommandResponseStatus.Success;
        }

        public static bool NotFound(this CommandResponse result) {
            return result.Status == CommandResponseStatus.NotFound;
        }

        public static bool Forbidden(this CommandResponse result) {
            return result.Status == CommandResponseStatus.Forbidden;
        }
    }

    public class CommandResponse<T> : CommandResponse {
        public T Subject { get; private set; }

        public CommandResponse<T> WithSubject(T subject) {
            Subject = subject;
            return this;
        }

        public new CommandResponse<T> WithMessage(string message) {
            base.WithMessage(message);
            return this;
        }

        public new CommandResponse<T> WithErrorMessage(string message) {
            base.WithErrorMessage(message);
            return this;
        }

        public new CommandResponse<T> WithError(string property, string message) {
            base.WithError(property, message);
            return this;
        }

        public new CommandResponse<T> WithError(ValidationResult error) {
            base.WithError(error);
            return this;
        }

        public new CommandResponse<T> Subsume(CommandResponse result) {
            base.Subsume(result);
            return this;
        }
    }

    public class CommandResponse {
        const string DefaultFailureMessage = "The following problems were found:";

        public string Status { get; set; }
        public string Message { get; set; }
        public IList<CommandResponseDetail> Details { get; private set; }

        public CommandResponse() {
            Details = new List<CommandResponseDetail>();
            Status = CommandResponseStatus.Success;
        }

        public CommandResponse WithMessage(string message) {
            Message = message;
            return this;
        }

        public CommandResponse WithErrorMessage(string message) {
            Message = message;
            Status = CommandResponseStatus.Failure;
            return this;
        }

        public CommandResponse WithError(string property, string message) {
            if(string.IsNullOrEmpty(Message)) {
                Message = DefaultFailureMessage;
            }

            Status = CommandResponseStatus.Failure;
            Details.Add(new CommandResponseDetail {
                Name = property,
                Description = message
            });

            return this;
        }

        public CommandResponse WithError(ValidationResult error) {
            return WithError(string.Join(", ", error.MemberNames), error.ErrorMessage);
        }

        public CommandResponse Subsume(CommandResponse subject) {
            if(subject.Status == CommandResponseStatus.Failure) {
                Status = CommandResponseStatus.Failure;
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

        public TResult To<TResult>() where TResult : CommandResponse, new() {
            var result = new TResult();
            result.Subsume(this);
            return result;
        }

        public static CommandResponse NotFound() {
            return new CommandResponse {Status = CommandResponseStatus.NotFound};
        }

        public static CommandResponse Forbidden() {
            return new CommandResponse {Status = CommandResponseStatus.Forbidden};
        }

        public static TResult NotFound<TResult>() where TResult : CommandResponse, new() {
            return new TResult {Status = CommandResponseStatus.NotFound};
        }

        public static TResult Forbidden<TResult>() where TResult : CommandResponse, new() {
            return new TResult {Status = CommandResponseStatus.Forbidden};
        }

        public static CommandResponse SuccessMessage(string message) {
            return new CommandResponse().WithMessage(message);
        }

        public static TResult ErrorMessage<TResult>(string message) where TResult : CommandResponse, new() {
            return ErrorMessage(message).To<TResult>();
        }

        public static CommandResponse ErrorMessage(string message) {
            return new CommandResponse().WithErrorMessage(message);
        }

        public static TResult Error<TResult>(string name, string description) where TResult : CommandResponse, new() {
            return Error(name, description).To<TResult>();
        }

        public static CommandResponse Error(string name, string description) {
            return new CommandResponse().WithError(name, description);
        }

        public static CommandResponse<T> Of<T>(T subject) {
            return new CommandResponse<T>().WithSubject(subject);
        }

        public class CommandResponseDetail {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}