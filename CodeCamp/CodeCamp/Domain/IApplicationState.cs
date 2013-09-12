using CodeCamp.Domain.Model;

namespace CodeCamp.Domain {
    public interface IApplicationState {
        User User { get; }
        Event CurrentEvent { get; }
        RegistrationStatus RegistrationStatus { get; set; }

        string Environment { get; }

        void Login(User user, bool persist);
        void Logout();

        void ChangeCurrentEvent(Event currentEvent);
    }
}