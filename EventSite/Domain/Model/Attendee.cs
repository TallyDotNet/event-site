using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventSite.Domain.Model
{
    public class Attendee {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public User User { get; set; }
    }
}