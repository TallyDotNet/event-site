namespace Castra.Web.Models
{
	using System;
	using BlueSpire.Kernel.Data;
	using BlueSpire.NHibernate;

	public class SessionProposal : Entity<Guid>
	{
		public virtual string Title { get; set; }
		public virtual string Abstract { get; set; }
		public virtual AudienceLevel AudienceLevel { get; set; }
		public virtual Profile Speaker { get; set; }

		public override string ToString()
		{
			return Title;
		}
	}

	public class SessionProposalMap : EntityMap<SessionProposal>
	{
		public SessionProposalMap()
		{
			Map(x => x.Title);
			Map(x => x.Abstract).Length(8000);
			Map(x => x.AudienceLevel);
			References(x => x.Speaker);
		}
	}
}