namespace Castra.Web.Commands
{
	using System;
	using BlueSpire.Kernel;
    using BlueSpire.Kernel.Bus;
    using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using BlueSpire.Web.Mvc.Membership;
	using Caliburn.Core.Validation;
	using Models;

    public class CreateUserHandler : CommandHandler<CreateUser>
    {
        private readonly IContextProvider _context;
        private readonly IRepository _repository;

        public CreateUserHandler(IContextProvider context, IRepository repository, IValidator validator)
            : base(validator)
        {
            _context = context;
            _repository = repository;
        }

        private User CreateUser(CreateUser command)
        {
            return new User
                       {
                           Username = command.Username.ToLower(),
                           Password = command.Password.Hash(),
                           Role = command.Role,
                           MemberSince = _context.Time,
                           LastLogin = _context.Time,
                           RegistrationIPAddress = _context.IPAddress,
                           Status = UserStatus.Active,
                       };
        }

        protected override Result Handle(CreateUser command)
        {
            var newUser = CreateUser(command);

            _repository.Add(newUser);

			_context.SetUser(newUser, false);

            return Success();
        }
    }
}