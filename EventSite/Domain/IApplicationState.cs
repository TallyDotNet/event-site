using EventSite.Domain.Model;

namespace EventSite.Domain {
    public interface IApplicationState {
        User User { get; }
        Event CurrentEvent { get; }
        RegistrationStatus RegistrationStatus { get; set; }
        ISettings Settings { get; }

        void Login(User user, bool persist);
        void Logout();

        void ChangeCurrentEvent(Event currentEvent);
    }
}