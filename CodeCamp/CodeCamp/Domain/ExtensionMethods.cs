using CodeCamp.Domain.Model;

namespace CodeCamp.Domain {
    public static class ExtensionMethods {
        public static bool IsLoggedIn(this IApplicationState state) {
            return state.User != null;
        }

        public static bool IsAdmin(this IApplicationState state) {
            return state.IsLoggedIn() && state.User.Roles.Contains(Roles.Admin);
        }
    }
}