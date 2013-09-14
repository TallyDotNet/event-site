﻿using Autofac;

namespace CodeCamp.Domain.Infrastructure {
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
    }
}