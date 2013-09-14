using System;
using EventSite.Domain.Infrastructure;

namespace EventSite.Domain.Model {
    public class Registration {
        public string Id { get; set; }
        public Reference User { get; set; }
        public Reference Event { get; set; }
        public DateTimeOffset RegisteredOn { get; set; }
        public bool IsSpeaker { get; set; }

        public static string IdFrom(string eventSlug, string userSlug) {
            if(string.IsNullOrEmpty(userSlug) || string.IsNullOrEmpty(eventSlug)) {
                return null;
            }

            return Model.Event.IdFrom(eventSlug) + "/registrations/" + userSlug;
        }
    }
}