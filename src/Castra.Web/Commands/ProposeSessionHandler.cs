namespace Castra.Web.Commands
{
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using Caliburn.Core.Validation;
	using Models;

	public class ProposeSessionHandler : CommandHandler<ProposeSession>
	{
		private readonly IContextProvider _context;
		private readonly IRepository _repository;

		public ProposeSessionHandler(IContextProvider context, IRepository repository, IValidator validator) : base(validator)
		{
			_context = context;
			_repository = repository;
		}

		protected override Result Handle(ProposeSession command)
		{
			var profile = _repository.Get(new QueryById<Profile> {Id = _context.User.Id});

			var proposal = new SessionProposal
			               	{
			               		Speaker = profile,
			               		Title = command.Title,
			               		Abstract = command.Abstract,
			               		AudienceLevel = command.AudienceLevel
			               	};

			_repository.Add(proposal);

			return Success();
		}
	}
}