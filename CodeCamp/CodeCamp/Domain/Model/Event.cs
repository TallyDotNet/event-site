namespace CodeCamp.Domain.Model {
    public class Event {
        public string Id { get; set; }
        public string Name { get; set; }
        public Venue Venue { get; private set; }
        public bool IsCurrent { get; set; }
        public bool IsSessionSubmissionOpen { get; set; }

        public Event() {
            Venue = new Venue();
        }
    }
}