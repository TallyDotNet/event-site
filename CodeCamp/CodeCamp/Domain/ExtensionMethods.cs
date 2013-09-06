using System;
using CodeCamp.Domain.Model;

namespace CodeCamp.Domain {
    public static class ExtensionMethods {
        public static bool UserIsLoggedIn(this IApplicationState state) {
            return state.User != null;
        }

        public static bool UserIsAdmin(this IApplicationState state) {
            return state.UserIsLoggedIn() && state.User.Roles.Contains(Roles.Admin);
        }

        public static bool RunningInProduction(this IApplicationState state) {
            return string.Compare(state.Environment, "production", StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}