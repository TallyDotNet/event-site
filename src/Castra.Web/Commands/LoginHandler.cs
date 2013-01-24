namespace Castra.Web.Commands
{
    using BlueSpire.Kernel;
    using BlueSpire.Kernel.Bus;
    using BlueSpire.Kernel.Data;
    using BlueSpire.Web.Mvc.Infrastructure;
    using BlueSpire.Web.Mvc.Membership;
    using Caliburn.Core.Validation;
    using Models;
    using Queries;

    public class LoginHandler : CommandHandler<Login>
    {
        private readonly IContextProvider context;
        private readonly IDataSource source;

        public LoginHandler(IContextProvider context, IDataSource source, IValidator validator)
            : base(validator)
        {
            this.context = context;
            this.source = source;
        }

        protected override Result Handle(Login command)
        {
            var username = command.Username;
            var hashedPassword = command.Password.Hash();

            var result = new Result();
            With.Transaction(
                () =>
                    {
                        var user = source.Get(new UserByLogin(username, hashedPassword));

                        if (user.NotFound())
                        {
                            result.AddError("InvalidCredentials", "The credentials supplied are invalid.");
                        }
                        else if (user.Status == UserStatus.Inactive)
                            result.AddError("InactiveUser", "The requested user account is not currently active.");
                        else
                        {
                            context.SetUser(user, command.RememberMe);
                        }
                    });

			return result;
        }
    }
}