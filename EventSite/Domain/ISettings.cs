namespace EventSite.Domain {
    public interface ISettings {
        string Environment { get; }
        string Name { get; }
        string Owner { get; }

        string FromEmail { get; }
        string FromEmailName { get; }

        string SmtpHost { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; }
        string SmtpPassword { get; }

        string BingMapsAPIKey { get; }

        string CommitId { get; }
    }
}