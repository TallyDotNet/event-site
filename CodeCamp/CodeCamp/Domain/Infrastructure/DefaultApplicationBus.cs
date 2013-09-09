using Raven.Client;

namespace CodeCamp.Domain.Infrastructure {
    public class DefaultApplicationBus : IApplicationBus {
        readonly IApplicationState state;
        readonly IDocumentSession docSession;

        public DefaultApplicationBus(IApplicationState state, IDocumentSession docSession) {
            this.state = state;
            this.docSession = docSession;
        }

        public T Execute<T>(ICommand<T> command) where T : Result {
            return command.Execute(this, state, docSession);
        }

        public TResult Query<TResult>(IQuery<TResult> query) {
            return query.Execute(this, state, docSession);
        }
    }
}