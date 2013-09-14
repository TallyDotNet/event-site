namespace CodeCamp.Domain.Infrastructure {
    public interface IApplicationBus {
        T Execute<T>(ICommand<T> command) where T : Result;
        TResult Query<TResult>(IQuery<TResult> query);
    }
}