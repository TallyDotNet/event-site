using System;
using System.Collections;
using System.Collections.Generic;
using EventSite.Domain;

namespace EventSite.Infrastructure {

    public class WebConfigSettings : ISettings {

        private readonly IConfigurationSettingsProvider settingsProvider;

        public WebConfigSettings(IConfigurationSettingsProvider settingsProvider) {
            this.settingsProvider = settingsProvider;
        }

        public string Environment {
            get { return settingsProvider.GetPropertyValue("EventSite.Environment"); }
        }

        private readonly IDictionary<string, AuthenticationModes> modeMap =
            new Dictionary<string, AuthenticationModes>(StringComparer.OrdinalIgnoreCase)
            {
                {"fake", AuthenticationModes.Fake},
                {"actual", AuthenticationModes.Actual}
            };

    public AuthenticationModes AuthenticationMode {
            get {
                var mode = AuthenticationModes.Default;
                var settingString = settingsProvider.GetPropertyValue("EventSite.AuthenticationMode");
                if(settingString != null)
                    modeMap.TryGetValue(settingString, out mode);
                
                return mode;
            }
        }

        public string Name {
            get { return settingsProvider.GetPropertyValue("EventSite.Name"); }
        }

        public string Owner {
            get { return settingsProvider.GetPropertyValue("EventSite.Owner"); }
        }

        public string FromEmail {
            get { return settingsProvider.GetPropertyValue("EventSite.FromEmail"); }
        }

        public string FromEmailName {
            get { return settingsProvider.GetPropertyValue("EventSite.FromEmailName"); }
        }

        public string SmtpHost {
            get { return settingsProvider.GetPropertyValue("Smtp.Host"); }
        }

        public int SmtpPort {
            get { return Convert.ToInt32(settingsProvider.GetPropertyValue("Smtp.Port")); }
        }

        public string SmtpUsername {
            get { return settingsProvider.GetPropertyValue("Smtp.Username"); }
        }

        public string SmtpPassword {
            get { return settingsProvider.GetPropertyValue("Smtp.Password"); }
        }

        public string BingMapsAPIKey {
            get { return settingsProvider.GetPropertyValue("EventSite.BingMapsAPIKey"); }
        }

        public string CommitId {
            get { return this.RunningInProduction() ? settingsProvider.GetPropertyValue("appharbor.commit_id") : "n/a"; }
        }
    }
}