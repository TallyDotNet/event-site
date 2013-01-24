namespace Castra.Web.Commands
{
	using System;
	using BlueSpire.Kernel.Bus;

	public class ApproveProposal : ICommand<Result>
	{
		public Guid ProposalId { get; private set; }

		public ApproveProposal(Guid proposalId)
		{
			ProposalId = proposalId;
		}
	}
}