namespace CodeCamp.Domain.Infrastructure {
    public interface IApplicationBus {
        ICommand<T> ExecuteCommand<T>(ICommand<T> command) where T : CommandResponse;
        TResult ExecuteQuery<TResult>(IQuery<TResult> query);
    }
}