using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using CodeCamp.Domain.Queries;

namespace CodeCamp.Domain.Commands {
    public class CreateAccount : Command<Result<User>> {
        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ExternalLoginData { get; set; }

        public ISecurityEncoder SecurityEncoder { get; set; }

        protected override Result<User> Execute() {
            string providerName, providerUserId;

            if(!SecurityEncoder.TryDeserializeOAuthProviderUserId(ExternalLoginData, out providerName, out providerUserId)) {
                return Error("Invalid external login data provided.");
            }

            var slug = SlugConverter.ToSlug(Username);

            if(prohibitSlug(slug)) {
                return Error("The requested username is not available.");
            }

            var user = Bus.Query(new UserWithEmail(Email));
            if(user != null) {
                return Error("An account already exists with the specified email address.");
            }

            user = new User {
                Id = User.IdFrom(slug),
                Email = Email,
                Username = Username,
                Preferences = {
                    ListInAttendeeDirectory = true,
                    ReceiveEmail = true
                }
            };

            user.AddOAuthAccount(providerName, providerUserId)
                .AddRole(Roles.User);

            if(string.Compare(user.Username, "EisenbergEffect", StringComparison.OrdinalIgnoreCase) == 0) {
                user.AddRole(Roles.Admin);
            }

            DocSession.Store(user);
            Log.Info("New user, {0}, created.", user.Username);

            return Result.Of(user)
                .WithMessage("Your account has been successfully created.");
        }

        static readonly string[] ForbiddenUserSlugs = {
            "admin", "administrator", "developer",
            "support", "owner", "siteowner", "site-owner", "bin"
        };

        bool prohibitSlug(string slug) {
            if(string.IsNullOrWhiteSpace(slug)) {
                return true;
            }

            var match = ForbiddenUserSlugs
                .FirstOrDefault(x => string.Compare(x, slug, StringComparison.OrdinalIgnoreCase) == 0);

            if(match != null) {
                return true;
            }

            return DocSession.Load<User>(User.IdFrom(slug)) != null;
        }
    }
}