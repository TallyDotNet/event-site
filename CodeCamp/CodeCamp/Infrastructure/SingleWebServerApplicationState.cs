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
            get {
                return new User {
                    Name = "Rob Eisenberg",
                    Roles = {Roles.User, Roles.Admin},
                    Email = "rob@bluespire.com",
                    TwitterHandle = "EisenbergEffect"
                };
            }
        }

        public ScheduledEvent CurrentEvent { get; private set; }

        public string Environment {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }
    }
}