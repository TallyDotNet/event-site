using System.Configuration;
using CodeCamp.Domain;
using CodeCamp.Domain.Model;
using Raven.Client;

namespace CodeCamp.Infrastructure {
    public class SingleWebServerApplicationState : IApplicationState {
        IDocumentSession docSession;

        public SingleWebServerApplicationState(IDocumentSession docSession) {
            this.docSession = docSession;
        }

        public User User {
            get { return null; }
        }

        public ScheduledEvent UpcomingEvent { get; private set; }

        public string Environment {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }

        public void Login(User user, bool persist) {
            throw new System.NotImplementedException();
        }

        public void Logout() {
            throw new System.NotImplementedException();
        }
    }
}