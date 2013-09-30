using EventSite.Domain.Infrastructure;

namespace EventSite.Domain.Model {
    public class Sponsor {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public Reference Event { get; set; }
        public SponsorshipLevel Level { get; set; }
        public int Priority { get; set; }
        public decimal AmountDonated { get; set; }
        public string ItemsDonated { get; set; }
        public string ImageSource { get; set; }

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