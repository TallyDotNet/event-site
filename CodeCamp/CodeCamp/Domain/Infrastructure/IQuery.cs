using NLog;
using Raven.Client;

namespace CodeCamp.Domain.Infrastructure {
    public interface IQuery<out TResult> {
        TResult Process();
    }

    public abstract class Query<TResult> : IQuery<TResult> {
        protected Logger Log { get; set; }
        public IApplicationBus Bus { get; set; }
        public IApplicationState State { get; set; }
        public IDocumentSession DocSession { get; set; }

        protected Query() {
            Log = LogManager.GetLogger(GetType().FullName);
        }

        public TResult Process() {
            return Execute();
        }

        protected abstract TResult Execute();
    }
}