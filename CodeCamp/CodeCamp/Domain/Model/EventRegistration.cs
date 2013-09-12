using System;
using CodeCamp.Domain.Infrastructure;

namespace CodeCamp.Domain.Model {
    public class EventRegistration {
        public string Id { get; set; }
        public Reference User { get; set; }
        public Reference Event { get; set; }
        public DateTimeOffset RegisteredOn { get; set; }
        public bool IsSpeaker { get; set; }
    }
}