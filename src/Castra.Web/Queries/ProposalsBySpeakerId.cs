namespace Castra.Web.Queries
{
	using System;
	using BlueSpire.Kernel.Data;
	using Models;

	public class ProposalsBySpeakerId : IPagedQuery<SessionProposal>
	{
		public ProposalsBySpeakerId(Guid speakerId)
		{
			SpeakerId = speakerId;
			PageNumber = 1;
			PageSize = 25;
		}

		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }

		public Guid SpeakerId { get; private set; }
	}
}