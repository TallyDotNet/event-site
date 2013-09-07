namespace CodeCamp.Domain.Infrastructure {
    public interface IApplicationBus {
        T Execute<T>(ICommand<T> command) where T : CommandResponse;
        TResult Query<TResult>(IQuery<TResult> query);
    }
}