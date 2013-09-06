namespace CodeCamp.Domain.Model {
    public class CodeCamp {
        public string Id { get; set; }
        public int Number { get; set; }
        public string Year { get; set; }
        public Venue Venue { get; set; }
        public bool IsCurrent { get; set; }
    }
}