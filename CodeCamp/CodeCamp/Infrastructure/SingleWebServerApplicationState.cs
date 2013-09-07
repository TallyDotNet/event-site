using System.Configuration;
using CodeCamp.Domain;
using CodeCamp.Domain.Model;

namespace CodeCamp.Infrastructure {
    public class SingleWebServerApplicationState : IApplicationState {
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