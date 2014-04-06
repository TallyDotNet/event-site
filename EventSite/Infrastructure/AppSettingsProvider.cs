using System.Configuration;

namespace EventSite.Infrastructure {

    public interface IConfigurationSettingsProvider {
        string GetPropertyValue(string key);
    }

    public class AppSettingsProvider : IConfigurationSettingsProvider {
        public string GetPropertyValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }
    }
}