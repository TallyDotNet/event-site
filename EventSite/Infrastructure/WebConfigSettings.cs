using System.Configuration;
using EventSite.Domain;

namespace EventSite.Infrastructure {
    public class WebConfigSettings : ISettings {
        public string Environment {
            get { return ConfigurationManager.AppSettings["EventSite.Environment"]; }
        }

        public string Name {
            get { return ConfigurationManager.AppSettings["EventSite.Name"]; }
        }

        public string Owner {
            get { return ConfigurationManager.AppSettings["EventSite.Owner"]; }
        }
    }
}