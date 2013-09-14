using System.Configuration;
using EventSite.Domain;

namespace EventSite.Infrastructure {
    public class WebConfigSettings : ISettings {
        public string Environment {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }
    }
}