namespace EventSite.Domain.Model {
    public class Attendee {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool ListInDirectory { get; set; }
    }
}