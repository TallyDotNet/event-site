using System;

namespace CodeCamp.Domain.Infrastructure {
    public class DefaultApplicationBus : IApplicationBus {
        public ICommand<T> ExecuteCommand<T>(ICommand<T> command) where T : CommandResponse {
            throw new NotImplementedException();
        }

        public TResult ExecuteQuery<TResult>(IQuery<TResult> query) {
            throw new NotImplementedException();
        }
    }
}