﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;

namespace EventSite.Domain.Commands {
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

        public ISlugConverter SlugConverter { get; set; }

        protected override Result<User> Execute() {
            string providerName, providerUserId;

            if(!SecurityEncoder.TryDeserializeOAuthProviderUserId(ExternalLoginData, out providerName, out providerUserId)) {
                return Error("Invalid external login data provided.");
            }

            var slug = SlugConverter.ToSlug(Username);

            if(prohibitSlug(slug)) {
                return Error(EventSiteResources.CreateUser_UsernameNotAvailable);
            }

            var user = Bus.Query(new UserWithEmail(Email));
            if(user != null) {
                return Error("An account already exists with the specified email address.");
            }

            user = new User {
                Id = User.IdFrom(slug),
                Email = Email,
                Username = Username,
                Status = UserStatus.Active,
                Preferences = {
                    ListInAttendeeDirectory = true,
                    ReceiveEmail = true
                }
            };

            user.AddOAuthAccount(providerName, providerUserId);

            user.AddRole(Roles.User);
            if(shouldMakeAdmin(Username)){
                user.AddRole(Roles.Admin);
            }

            DocSession.Store(user);
            Log.Info("New user, {0}, created.", user.Username);

            return Result.Of(user)
                .WithMessage("Your account has been successfully created.");
        }

        public static readonly string[] ForbiddenUserSlugs = {
            "admin", "administrator", "developer",
            "support", "owner", "siteowner", "site-owner", "bin"
        };

        bool shouldMakeAdmin(string username) {
            //this method allows for a couple of ways to bootstrap the initial admin user
            //for a fresh installation of the app where no users exist yet.

            if (username.Equals(State.Settings.InitialAdminUserName, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!State.RunningInProduction() && !Bus.Query(new AdminUserExists()))
                return true;

            return false;
        }

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