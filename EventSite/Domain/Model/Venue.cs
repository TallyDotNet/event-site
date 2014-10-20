using RestSharp.Extensions;

namespace EventSite.Domain.Model {
    public class Venue {
        public string Name { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string VenueInfoLink { get; set; }
        public string ParkingInformation { get; set; }
        public string ParkingInfoLink { get; set; }

        public bool HasAddressSet() {
            return Street1.HasValue() &&
                   City.HasValue() &&
                   StateOrProvince.HasValue() &&
                   PostalCode.HasValue();
        }
    }
}