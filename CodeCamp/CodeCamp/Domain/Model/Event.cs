namespace CodeCamp.Domain.Model {
    public class Event {
        public string Id { get; set; }
        public string Name { get; set; }
        public Venue Venue { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsSessionSubmissionOpen { get; set; }
    }
}