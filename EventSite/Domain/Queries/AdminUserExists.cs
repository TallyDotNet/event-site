using System;
using System.Globalization;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;

namespace EventSite.Domain.Queries {
    public class AdminUserExists : Query<bool> {
        protected override bool Execute() {
            return DocSession.Query<User>()
                .Any(user => user.Roles.Any(r => r.Equals(Roles.Admin.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase)));       
        }
    }
}