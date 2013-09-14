using System;

namespace CodeCamp.Domain.Model {
    public class Event {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Venue Venue { get; set; }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }

        public bool IsCurrent { get; set; }
        public bool IsSessionSubmissionOpen { get; set; }

        public Event() {
            Venue = new Venue();
        }

        public static string IdFrom(string slug) {
            if(string.IsNullOrEmpty(slug)) {
                return null;
            }

            return "events/" + slug.ToLower();
        }

        public static string SlugFromId(string id) {
            return string.IsNullOrEmpty(id) ? null : id.Replace("events/", "");
        }
    }
}