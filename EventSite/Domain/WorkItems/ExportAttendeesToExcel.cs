using System;
using System.Collections.Generic;
using System.IO;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;

namespace EventSite.Domain.WorkItems {
    public class ExportAttendeesToExcel : ExportToExcel<Attendee> {
        readonly IEnumerable<Attendee> attendees;
        readonly FileInfo targetFile;
        readonly IDictionary<string, Func<Attendee, object>> columns;

        public ExportAttendeesToExcel(IEnumerable<Attendee> attendees, FileInfo targetFile) {
            this.attendees = attendees;
            this.targetFile = targetFile;

            columns = new Dictionary<string, Func<Attendee, object>> {
                {"Username", attendee => attendee.User.Username},
                {"DisplayName", attendee => attendee.DisplayName},
                {"Email", attendee => attendee.User.Email},
                {"ReceiveEmail", attendee => attendee.User.Preferences.ReceiveEmail},
                {"ListInAttendeeDirectory", attendee => attendee.User.Preferences.ListInAttendeeDirectory}
            };
        }

        protected override IEnumerable<Attendee> DataSource {
            get { return attendees; }
        }

        protected override IDictionary<string, Func<Attendee, object>> Columns {
            get { return columns; }
        }

        protected override FileInfo TargetFile {
            get { return targetFile; }
        }
    }
}