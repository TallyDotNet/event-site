using System.Collections.Generic;

namespace CodeCamp.Domain.Model {
    public class User {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserProfile Profile { get; private set; }
        public UserPreferences Preferences { get; private set; }
        public IList<string> Roles { get; private set; }
        public IList<OAuthAccount> OAuthAccounts { get; private set; }
        //TODO: UserStatus

        public User() {
            Profile = new UserProfile();
            Preferences = new UserPreferences();
            Roles = new List<string>();
            OAuthAccounts = new List<OAuthAccount>();
        }

        public User AddOAuthAccount(string providerName, string providerUserId) {
            OAuthAccounts.Add(new OAuthAccount {Provider = providerName, ProviderUserId = providerUserId});
            return this;
        }

        public bool InRole(string role) {
            return Roles.Contains(role);
        }

        public User AddRole(string role) {
            if(!Roles.Contains(role)) {
                Roles.Add(role);
            }

            return this;
        }

        public static string IdFrom(string slug) {
            if(string.IsNullOrEmpty(slug)) {
                return null;
            }

            return "users/" + slug.ToLower();
        }

        public class OAuthAccount {
            public string Provider { get; set; }
            public string ProviderUserId { get; set; }
        }

        public class UserPreferences {
            public bool ListInAttendeeDirectory { get; set; }
            public bool ReceiveEmail { get; set; }
        }

        public class UserProfile {
            public string Name { get; set; }
            public string Title { get; set; }
            public string Company { get; set; }
            public string Bio { get; set; }
            public string TelephoneNumber { get; set; }
            //public string BlogUrl { get; set; }
            //public string TwitterHandle { get; set; }
            //public bool IsMVP { get; set; }
        }
    }
}