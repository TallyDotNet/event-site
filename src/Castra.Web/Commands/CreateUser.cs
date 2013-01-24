namespace Castra.Web.Commands
{
    using System.Collections.Generic;
    using BlueSpire.Kernel;
    using BlueSpire.Kernel.Bus;
    using BlueSpire.Kernel.Data;
    using BlueSpire.Web.Mvc.Membership;
    using FluentValidation;
    using Models;
    using Queries;

	public class CreateUser : ICommand<Result>
    {
        private string _password;
        private string _passwordVerification;
        private UserRole _role;
        private string _username;
        private bool _verifiedAsHuman;

        public string Username
        {
            get { return _username; }
            set { _username = value.Clean(); }
        }

        public UserRole Role
        {
            get { return _role ?? UserRole.Member; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value.Clean(); }
        }

        public string PasswordVerification
        {
            get { return _passwordVerification; }
            set { _passwordVerification = value.Clean(); }
        }

        public bool VerifiedAsHuman
        {
            get { return _verifiedAsHuman; }
        }

        public CreateUser WithRole(UserRole role)
        {
            _role = role;
            return this;
        }

        public void VerifyAsHuman()
        {
            _verifiedAsHuman = true;
        }

        #region Nested type: Validator

        public class Validator : AbstractValidator<CreateUser>
        {
            public static List<string> ReservedNames = new List<string>
                                                           {
                                                               "admin",
                                                               "administrator",
                                                               "master",
                                                               "moderator",
                                                               "god",
                                                               "super",
                                                               "guest",
                                                               "visitor"
                                                           };

            public Validator(IDataSource source)
            {
                RuleFor(x => x.Username)
                    .Matches(Email.PatternString)
                    .NotEmpty();

                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.PasswordVerification).NotEmpty();

                RuleFor(x => x.Username).Must( x=> !ReservedNames.Contains(x))
                    .WithMessage("Sorry, that email address has already been registered.");

                RuleFor(x => x.Username).Must(x => source.Get(new IsUsernameAvailable(x)))
					.WithMessage("Sorry, that email address has already been registered.");
            }
        }

        #endregion
    }
}