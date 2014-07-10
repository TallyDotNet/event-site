namespace EventSite.Domain {
    public interface ISettings {
        string Environment { get; }
        AuthenticationModes AuthenticationMode { get; }
        string Name { get; }
        string Owner { get; }
        string InitialAdminUserName { get; }

        string FromEmail { get; }
        string FromEmailName { get; }

        string SmtpHost { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; }
        string SmtpPassword { get; }

        string BingMapsAPIKey { get; }

        string CommitId { get; }
    }

    public enum AuthenticationModes {
        Default, //uses "fake" auth providers anywhere but prod, and real ones in prod
        Fake, //uses the "fake" auth providers reglardless of the environment settings
        Actual //uses the actual auth providers regardless of the environment settings
    }

}