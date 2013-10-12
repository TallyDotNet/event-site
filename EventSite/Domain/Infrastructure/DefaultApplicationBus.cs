using Autofac;

namespace EventSite.Domain.Infrastructure {
    public class DefaultApplicationBus : IApplicationBus {
        readonly IComponentContext container;

        public DefaultApplicationBus(IComponentContext container) {
            this.container = container;
        }

        public T Execute<T>(ICommand<T> command) where T : Result {
            container.InjectProperties(command);
            return command.Process();
        }

        public TResult Query<TResult>(IQuery<TResult> query) {
            container.InjectProperties(query);
            return query.Process();
        }

        public void Enqueue(IWork work) {
            //we don't care to actually put this on a queue for this simple app right now
            //so we will just process it immediately
            container.InjectProperties(work);
            work.Process(); 
        }

        public void Do(IWork work) {
            container.InjectProperties(work);
            work.Process();
        }
    }
}