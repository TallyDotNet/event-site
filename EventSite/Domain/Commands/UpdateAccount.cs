using System.ComponentModel.DataAnnotations;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;

namespace EventSite.Domain.Commands {
    public class UpdateAccount : Command<Result> {
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string Company { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [Phone]
        public string TelephoneNumber { get; set; }
        
        [StringLength(5000)]
        public string Bio { get; set; }

        public bool ListInAttendeeDirectory { get; set; }

        public bool ReceiveEmail { get; set; }

        protected override Result Execute() {
            var user = DocSession.Load<User>(CurrentUser.Id);

            CurrentUser.Email = user.Email = Email;

            CurrentUser.Profile.Name = user.Profile.Name = Name;
            CurrentUser.Profile.Company = user.Profile.Company = Company;
            CurrentUser.Profile.Title = user.Profile.Title = Title;
            CurrentUser.Profile.TelephoneNumber = user.Profile.TelephoneNumber = TelephoneNumber;
            CurrentUser.Profile.Bio = user.Profile.Bio = Bio;

            CurrentUser.Preferences.ListInAttendeeDirectory = user.Preferences.ListInAttendeeDirectory = ListInAttendeeDirectory;
            CurrentUser.Preferences.ReceiveEmail = user.Preferences.ReceiveEmail = ReceiveEmail;

            return Success("Your account has been successfully updated.");
        }
    }
}