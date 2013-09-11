using System;
using CodeCamp.Domain.Infrastructure;

namespace CodeCamp.Domain.Model {
    public class Session {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AudienceLevel Level { get; set; }
        public Reference Event { get; set; }
        public Reference Submitter { get; set; }
        public DateTimeOffset SubmittedOn { get; set; }
        public SessionStatus Status { get; set; }
    }
}