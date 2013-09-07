using CodeCamp.Domain.Model;

namespace CodeCamp.Domain {
    public interface IApplicationState {
        User User { get; }
        ScheduledEvent CurrentEvent { get; }
        string Environment { get; }
    }
}