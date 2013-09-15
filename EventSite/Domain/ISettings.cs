namespace EventSite.Domain {
    public interface ISettings {
        string Environment { get; }
        string Name { get; }
        string Owner { get; }
    }
}