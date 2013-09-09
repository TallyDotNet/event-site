using CodeCamp.Domain.Infrastructure;

namespace CodeCamp.Infrastructure.Views {
    public class ViewInfo {
        public string Title { get; set; }
        public Result Response { get; private set; }

        public void Report(Result result) {
            Response = result;
        }

        public void ReportSuccess(string message) {
            Report(Result.SuccessMessage(message));
        }

        public void ReportError(string message) {
            Report(Result.ErrorMessage(message));
        }
    }
}