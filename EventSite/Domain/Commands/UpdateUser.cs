using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;

namespace EventSite.Domain.Commands
{
    public class UpdateUser : Command<Result>.AdminOnly {

        private readonly User user;

        public UpdateUser(User user) {
            this.user = user;
        }

        public User User {
            get { return user; }
        }

        public bool InAdminRole {
            get { return InRole(Roles.Admin); }
            set { SetRole(Roles.Admin, value); }
        }

        public bool InSponsorManagerRole {
            get { return InRole(Roles.ManageSponsors); }
            set { SetRole(Roles.ManageSponsors, value); }
        }

        private bool InRole(string roleName) {
            return user.InRole(roleName);
        }

        protected override Result Execute() {
            var userToUpdate = DocSession.Load<User>(User.Id);

            if (userToUpdate == null) {
                return Error("The user was not found in the database.");
            }

            var newRolesForUser = userToUpdate.Roles.Intersect(User.Roles);

            

            foreach (var role in userToUpdate.Roles)
            {
                if (role == Roles.User)
                    continue;

                if (!userToUpdate.Roles.Contains(role))
                {
                    
                }
            }

            return null;
        }

        private void SetRole(string roleName, bool inRole)
        {
            if (inRole && !user.InRole(roleName))
            {
                user.AddRole(roleName);
            }
            else if (!inRole && user.InRole(roleName))
            {
                user.RemoveRole(roleName);
            }
        }
    }
}