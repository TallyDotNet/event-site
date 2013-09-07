using CodeCamp.Domain.Infrastructure;

namespace CodeCamp.Infrastructure.Views {
    public class ViewInfo {
        public string Title { get; set; }
        public CommandResponse Response { get; private set; }

        public void Report(CommandResponse result) {
            Response = result;
        }

        public void ReportSuccess(string message) {
            Report(CommandResponse.SuccessMessage(message));
        }

        public void ReportError(string message) {
            Report(CommandResponse.ErrorMessage(message));
        }
    }
}