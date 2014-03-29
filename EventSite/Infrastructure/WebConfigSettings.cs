using System;
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

        public string FromEmail {
            get { return ConfigurationManager.AppSettings["EventSite.FromEmail"]; }
        }

        public string FromEmailName {
            get { return ConfigurationManager.AppSettings["EventSite.FromEmailName"]; }
        }

        public string SmtpHost {
            get { return ConfigurationManager.AppSettings["Smtp.Host"]; }
        }

        public int SmtpPort {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["Smtp.Port"]); }
        }

        public string SmtpUsername {
            get { return ConfigurationManager.AppSettings["Smtp.Username"]; }
        }

        public string SmtpPassword {
            get { return ConfigurationManager.AppSettings["Smtp.Password"]; }
        }

        public string BingMapsAPIKey {
            get { return ConfigurationManager.AppSettings["EventSite.BingMapsAPIKey"]; }
        }

        public string CommitId {
            get { return this.RunningInProduction() ? ConfigurationManager.AppSettings["appharbor.commit_id"] : "n/a"; }
        }
    }
}