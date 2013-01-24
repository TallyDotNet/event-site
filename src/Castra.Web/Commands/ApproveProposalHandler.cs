namespace Castra.Web.Commands
{
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using Caliburn.Core.Validation;
	using Models;

	public class ApproveProposalHandler : CommandHandler<ApproveProposal>
	{
		private readonly IContextProvider _context;
		private readonly IRepository _repository;

		public ApproveProposalHandler(IContextProvider context, IRepository repository, IValidator validator)
			: base(validator)
		{
			_context = context;
			_repository = repository;
		}

		protected override Result Handle(ApproveProposal command)
		{
			var proposal = _repository.Get(new QueryById<SessionProposal> {Id = command.ProposalId});

			var session = new Session
			              	{
			              		Title = proposal.Title,
			              		Abstract = proposal.Abstract,
			              		AudienceLevel = proposal.AudienceLevel,
			              		Speaker = proposal.Speaker
			              	};

			session.Speaker.HasApprovedSessions = true;

			_repository.Delete<SessionProposal>(command.ProposalId);
			_repository.Add(session);

			return Success();
		}
	}
}