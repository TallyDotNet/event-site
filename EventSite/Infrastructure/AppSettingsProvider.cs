using System.Configuration;

namespace EventSite.Infrastructure {

    public interface IAppSettingsProvider {
        string GetPropertyValue(string key);
    }

    public class AppSettingsProvider : IAppSettingsProvider {
        public string GetPropertyValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }
    }
}