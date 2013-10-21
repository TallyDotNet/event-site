using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace EventSite.Domain.Infrastructure {
    public class Page {
        public const int DefaultPageSize = 25;
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
            get { return TotalPages > 1; }
        }

        public static int CalculatePages(int totalResults, int pageSize = DefaultPageSize) {
            var floatingPointResult = (decimal) totalResults/pageSize;
            return (int) Math.Ceiling(floatingPointResult);
        }

        public static IQueryable<T> Transform<T>(IRavenQueryable<T> query, ref int page, out RavenQueryStatistics statistics, int pageSize = DefaultPageSize) {
            if(page < 1) {
                page = 1;
            }

            return query.Statistics(out statistics)
                .Skip((page - 1)*pageSize)
                .Take(pageSize);
        }
    }

    public class Page<T> : Page {
        public IEnumerable<T> Items { get; set; }
    }
}