using System;
using System.Collections.Generic;
using EventSite.Infrastructure.Data.Export;

namespace EventSite.Domain.Model {
    public class SponsorsExportColumnMappings : IExportColumnMappings<Sponsor> {
        public IDictionary<string, Func<Sponsor, object>> Columns {
            get {
                return new Dictionary<string, Func<Sponsor, object>>
                {
                    {"Name", sponsor => sponsor.Name},
                    {"Level", sponsor => sponsor.Level},
                    {"Description", sponsor=> sponsor.Description},
                    {"Link", sponsor => sponsor.Link},
                    {"Donated On", sponsor => sponsor.DonatedOn.HasValue ? sponsor.DonatedOn.Value.ToString("yyyy-MM-dd") : "N/A"},
                    {"Amount Donated", sponsor => sponsor.AmountDonated},
                    {"Items Donated", sponsor => sponsor.ItemsDonated}
                };
            }
        }
    }
}