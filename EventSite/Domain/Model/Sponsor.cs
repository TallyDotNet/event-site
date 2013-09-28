using EventSite.Domain.Infrastructure;

namespace EventSite.Domain.Model {
    public class Sponsor {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Reference Event { get; set; }

        public static string IdFrom(string eventSlug, string sponsorSlug) {
            if(string.IsNullOrEmpty(sponsorSlug) || string.IsNullOrEmpty(eventSlug)) {
                return null;
            }

            return Model.Event.IdFrom(eventSlug) + "/sponsors/" + sponsorSlug.ToLower();
        }

        public static string SlugFromId(string id) {
            return string.IsNullOrEmpty(id) ? null : id.Substring(id.LastIndexOf("/") + 1);
        }
    }
}