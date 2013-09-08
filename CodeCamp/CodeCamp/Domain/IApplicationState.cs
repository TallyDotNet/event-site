using CodeCamp.Domain.Model;

namespace CodeCamp.Domain {
    public interface IApplicationState {
        User User { get; }
        ScheduledEvent UpcomingEvent { get; }
        string Environment { get; }

        void Login(User user, bool persist);
        void Logout();
    }
}