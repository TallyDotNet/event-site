﻿using System;
using System.Collections.Generic;
using EventSite.Infrastructure.Data.Export;

namespace EventSite.Domain.Model {
    public class AttendeesExportColumnMappings : IExportColumnMappings<Attendee> {

        public IDictionary<string, Func<Attendee, object>> Columns {
            get {
                return new Dictionary<string, Func<Attendee, object>>
                {
                    {"Username", attendee => attendee.User.Username},
                    {"DisplayName", attendee => attendee.DisplayName},
                    {"Email", attendee => attendee.User.Email},
                    {"ReceiveEmail", attendee => attendee.User.Preferences.ReceiveEmail},
                    {"ListInAttendeeDirectory", attendee => attendee.User.Preferences.ListInAttendeeDirectory},
                    {"IsSpeaker" , attendee => attendee.IsSpeaker }
                };
            }
        }
    }
}