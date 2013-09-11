using System.ComponentModel.DataAnnotations;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;

namespace CodeCamp.Domain.Commands {
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
            CurrentUser.Email = Email;

            CurrentUser.Profile.Name = Name;
            CurrentUser.Profile.Company = Company;
            CurrentUser.Profile.Title = Title;
            CurrentUser.Profile.TelephoneNumber = TelephoneNumber;
            CurrentUser.Profile.Bio = Bio;

            CurrentUser.Preferences.ListInAttendeeDirectory = ListInAttendeeDirectory;
            CurrentUser.Preferences.ReceiveEmail = ReceiveEmail;

            return Success("Your account has been successfully updated.");
        }
    }
}