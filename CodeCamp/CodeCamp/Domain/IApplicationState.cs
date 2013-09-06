using CodeCamp.Domain.Model;

namespace CodeCamp.Domain {
    public interface IApplicationState {
        User User { get; }
        Model.CodeCamp CodeCamp { get; }
    }
}