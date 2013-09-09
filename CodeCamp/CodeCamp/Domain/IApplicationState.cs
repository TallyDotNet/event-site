using CodeCamp.Domain.Model;

namespace CodeCamp.Domain {
    public interface IApplicationState {
        User User { get; }
        Event UpcomingEvent { get; }
        string Environment { get; }

        void Login(User user, bool persist);
        void Logout();
    }
}