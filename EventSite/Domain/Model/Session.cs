using System;
using EventSite.Domain.Infrastructure;

namespace EventSite.Domain.Model {
    public class Session {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AudienceLevel Level { get; set; }
        public Reference Event { get; set; }
        public Reference User { get; set; }
        public DateTimeOffset SubmittedOn { get; set; }
        public SessionStatus Status { get; set; }

        public static string IdFrom(string eventSlug, string sessionSlug) {
            if(string.IsNullOrEmpty(sessionSlug) || string.IsNullOrEmpty(eventSlug)) {
                return null;
            }

            return Model.Event.IdFrom(eventSlug) + "/sessions/" + sessionSlug.ToLower();
        }

        public static string SlugFromId(string id) {
            if(string.IsNullOrEmpty(id)) {
                return null;
            }

            return id.Substring(id.IndexOf("/sessions/") + "/sessions/".Length);
        }
    }
}