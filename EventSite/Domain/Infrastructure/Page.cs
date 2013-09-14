using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace EventSite.Domain.Infrastructure {
    public class Page {
        public static int Size = 25;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public int NextPage {
            get { return CurrentPage + 1; }
        }

        public int PreviousPage {
            get { return CurrentPage - 1; }
        }

        public bool HasPreviousPage {
            get { return CurrentPage > 1; }
        }

        public bool HasNextPage {
            get { return CurrentPage < TotalPages; }
        }

        public bool HasMultiplePages {
            get {
                return TotalPages > 1;
            }
        }

        public static IQueryable<T> Transform<T>(IRavenQueryable<T> query, ref int page, out RavenQueryStatistics statistics) {
            if(page < 1) {
                page = 1;
            }

            return query.Statistics(out statistics)
                .Skip((page - 1)*Size)
                .Take(Size);
        }
    }

    public class Page<T> : Page {
        public IEnumerable<T> Items { get; set; }
    }
}