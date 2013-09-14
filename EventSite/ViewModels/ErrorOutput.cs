namespace EventSite.ViewModels {
    public class ErrorOutput {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }

        public string Title {
            get {
                switch(ErrorCode) {
                    case "404":
                        return "Not Found";
                    case "403":
                        return "Forbidden";
                    default:
                        return "Unknown Error";
                }
            }
        }

        public string Subtitle1 {
            get {
                switch(ErrorCode) {
                    case "404":
                        return "Looking for something?";
                    case "403":
                        return "No sir. That's not for your eyes.";
                    default:
                        return "Something unexpected has happened.";
                }
            }
        }

        public string Subtitle2 {
            get {
                switch(ErrorCode) {
                    case "404":
                        return "It's not here...";
                    case "403":
                        return "We're watching you ;)";
                    default:
                        return "If the problem persists, please contact us.";
                }
            }
        }
    }
}