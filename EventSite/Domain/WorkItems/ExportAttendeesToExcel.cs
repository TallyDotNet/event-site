using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;

namespace EventSite.Domain.WorkItems
{
    public class ExportAttendeesToExcel : ExportToExcel<Attendee> {
        private readonly IEnumerable<Attendee> attendees;
        private readonly FileInfo targetFile;
        private readonly IDictionary<string, Func<string>> columns;

        public ExportAttendeesToExcel(IEnumerable<Attendee> attendees, FileInfo targetFile) {
            this.attendees = attendees;

            this.targetFile = targetFile;
            columns = new Dictionary<string, Func<string>>
                {
                    {"Username", null},
                    {"DislayName", null},
                    {"Email", null},
                    {"ReceiveEmail", null},
                    {"ShowInListing", null}
                };
        }

        protected override IEnumerable<Attendee> DataSource {
            get { return attendees; }
        }

        protected override IDictionary<string, Func<string>> Columns {
            get { return columns; }
        }

        protected override FileInfo TargetFile {
            get { return targetFile; }
        }
    }
}