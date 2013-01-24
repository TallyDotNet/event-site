namespace Castra.Web.Commands
{
	using BlueSpire.Kernel;
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using BlueSpire.Web.Mvc.Membership;
	using Caliburn.Core.Validation;

	public class ChangePasswordHandler : CommandHandler<ChangePassword>
	{
		readonly IContextProvider context;
		readonly IRepository repository;

		public ChangePasswordHandler(IContextProvider context, IRepository repository, IValidator validator)
			: base(validator)
		{
			this.context = context;
			this.repository = repository;
		}

		protected override Result Handle(ChangePassword command)
		{
			if (command.NewPassword != command.PasswordVerification)
			{
				var r = new Result();
				r.AddError("PasswordsDidNotMatch", "The two passwords did not match.");
				return r;
			}
			var user = repository.Get(new QueryById<User> {Id = context.User.Id});
			user.Password = command.NewPassword.Hash();

			return Success();
		}
	}
}