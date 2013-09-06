using System.Collections.Generic;

namespace CodeCamp.Domain.Model {
    public class User {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string BlogUrl { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public bool IsMVP { get; set; }
        public string TwitterHandle { get; set; }
        public string Bio { get; set; }
        public string TelephoneNumber { get; set; }
        public bool ListInAttendeeDirectory { get; set; }
        public bool ReceiveEmail { get; set; }
        public IList<string> Roles { get; private set; }

        public User() {
            Roles = new List<string>();
        }
    }
}