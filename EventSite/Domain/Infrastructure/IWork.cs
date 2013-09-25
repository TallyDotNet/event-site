using EventSite.Domain.Model;
using NLog;
using Raven.Client;

namespace EventSite.Domain.Infrastructure {
    public interface IWork {
        void Process();
    }

    public abstract class Work : IWork {
        protected Logger Log { get; set; }
        public IApplicationBus Bus { get; set; }
        public IApplicationState State { get; set; }
        public IDocumentSession DocSession { get; set; }

        protected User CurrentUser {
            get { return State.User; }
        }

        protected Work() {
            Log = LogManager.GetLogger(GetType().FullName);
        }

        public abstract void Process();
    }
}