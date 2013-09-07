namespace CodeCamp.Domain.Infrastructure {
    public class DefaultApplicationBus : IApplicationBus {
        readonly IApplicationState state;

        public DefaultApplicationBus(IApplicationState state) {
            this.state = state;
        }

        public T Execute<T>(ICommand<T> command) where T : CommandResponse {
            return command.Execute(this, state);
        }

        public TResult Query<TResult>(IQuery<TResult> query) {
            return query.Execute(this, state);
        }
    }
}