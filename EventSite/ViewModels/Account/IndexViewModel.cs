using System.Collections.Generic;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;

namespace EventSite.ViewModels.Account {
    public class IndexViewModel : UpdateAccount {
        public IEnumerable<Session> SubmittedSessions { get; set; }

        public IndexViewModel WithUser(User user) {
            Email = user.Email;

            Name = user.Profile.Name;
            Company = user.Profile.Company;
            Title = user.Profile.Title;
            TelephoneNumber = user.Profile.TelephoneNumber;
            Bio = user.Profile.Bio;

            ListInAttendeeDirectory = user.Preferences.ListInAttendeeDirectory;
            ReceiveEmail = user.Preferences.ReceiveEmail;

            return this;
        }
    }
}