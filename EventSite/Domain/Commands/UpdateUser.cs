using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;

namespace EventSite.Domain.Commands
{
    public class UpdateUser : Command<Result>.AdminOnly {

        public User User { get; set; }

        public bool InAdminRole { get; set; }

        public bool InSponsorManagerRole { get; set; }

        protected override Result Execute() {
            var userToUpdate = DocSession.Load<User>(User.Id);

            if (userToUpdate == null) {
                return Error("The user was not found in the database.");
            }

            userToUpdate.Profile.Title = User.Profile.Title;
            userToUpdate.Profile.Bio = User.Profile.Bio;
            userToUpdate.Profile.Name = User.Profile.Name;
            userToUpdate.Profile.Company = User.Profile.Company;
            userToUpdate.Profile.TelephoneNumber = User.Profile.TelephoneNumber;

            userToUpdate.Roles.Clear();
            userToUpdate.AddRole(Roles.User);

            if (InAdminRole) {
                userToUpdate.AddRole(Roles.Admin);
            }

            if (InSponsorManagerRole) {
                userToUpdate.AddRole(Roles.ManageSponsors);
            }

            return SuccessFormat("User '{0}' was updated successfully.", userToUpdate.GetDisplayName());
        }
    }
}