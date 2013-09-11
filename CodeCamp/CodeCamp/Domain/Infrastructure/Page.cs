using System.Collections.Generic;

namespace CodeCamp.Domain.Infrastructure {
    public class Page {
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
    }

    public class Page<T> : Page {
        public IEnumerable<T> Items { get; set; }
    }
}