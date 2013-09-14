using System.Collections.Generic;
using EventSite.Domain.Infrastructure;

namespace EventSite.Domain.Model {
    public class Speaker {
        public string Id { get; set; }
        public string EventId { get; set; }
        public User User { get; set; }
        public IEnumerable<Reference> Sessions { get; set; }
    }
}