using EventSite.Domain.Commands;
using EventSite.Domain.Model;

namespace EventSite.Domain.WorkItems {
    public class SendSessionStatusChangeEmail : SendEmail {
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string SessionName { get; set; }
        public SessionStatus SessionStatus { get; set; }

        public SendSessionStatusChangeEmail(Event @event, Session session, User user) {
            ToName = user.GetDisplayName();
            ToEmail = user.Email;

            EventName = @event.Name;
            SessionName = session.Name;
            EventDate = @event.Start.DateTime.ToLongDateString();
            SessionStatus = session.Status;
            Subject = "Your " + EventName + " Session Has Been " + SessionStatus;
            Template = "SessionStatusChange";
        }
    }
}