using System.Collections.Generic;

namespace EventSite.Domain.Model {
    public class User {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserProfile Profile { get; private set; }
        public UserPreferences Preferences { get; private set; }
        public IList<string> Roles { get; private set; }
        public IList<AuthAccount> AuthAccounts { get; private set; }
        public UserStatus Status { get; set; }

        public User() {
            Profile = new UserProfile();
            Preferences = new UserPreferences();
            Roles = new List<string>();
            AuthAccounts = new List<AuthAccount>();
        }

        public User AddOAuthAccount(string providerName, string providerUserId) {
            AuthAccounts.Add(new AuthAccount {Provider = providerName, ProviderUserId = providerUserId});
            return this;
        }

        public string GetDisplayName() {
            return string.IsNullOrEmpty(Profile.Name) ? Username : Profile.Name;
        }

        public string GetProfessionalInfo() {
            var info = Profile.Title;

            if(!string.IsNullOrEmpty(info)) {
                if(!string.IsNullOrEmpty(Profile.Company)) {
                    info += ", " + Profile.Company;
                }
            } else {
                info = Profile.Company;
            }

            return info;
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

        public static string SlugFromId(string userId) {
            return string.IsNullOrEmpty(userId) ? null : userId.Replace("users/", string.Empty);
        }

        public class AuthAccount {
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
            public bool IsKeynoteSpeaker { get; set; }
            public UserProfile()
            {
            }
        }
    }
}