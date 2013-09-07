using NLog;
using Raven.Client;

namespace CodeCamp.Domain.Infrastructure {
    public interface IQuery<out TResult> {
        TResult Execute(IApplicationBus bus, IApplicationState state, IDocumentSession docSession);
    }

    public abstract class Query<TResult> : IQuery<TResult> {
        protected Logger Log { get; private set; }
        protected IApplicationBus Bus { get; private set; }
        protected IApplicationState State { get; private set; }
        protected IDocumentSession DocSession { get; private set; }

        public TResult Execute(IApplicationBus bus, IApplicationState state, IDocumentSession docSession) {
            Log = LogManager.GetLogger(GetType().FullName);
            Bus = bus;
            State = state;
            DocSession = docSession;

            return Execute();
        }

        protected abstract TResult Execute();
    }
}