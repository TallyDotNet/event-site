namespace Castra.Web.Models
{
	using System;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using BlueSpire.NHibernate;

	public class Session : Entity<Guid>
	{
		private string _title;

		public virtual string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				LookupName = _title.DeriveLookupName();
			}
		}

		public virtual string LookupName { get; private set; }
		public virtual string Abstract { get; set; }
		public virtual string SpeakerRateUrl { get; set; }
		public virtual AudienceLevel AudienceLevel { get; set; }
		public virtual Profile Speaker { get; set; }
		public virtual string Room { get; set; }
		public virtual DateTime? Time { get; set; }
		public virtual Track Track { get; set; }

		public override string ToString()
		{
			return Title;
		}
	}

	public class SessionMap : EntityMap<Session>
	{
		public SessionMap()
		{
			Map(x => x.Title);
			Map(x => x.LookupName);
			Map(x => x.Abstract).Length(8000);
			Map(x => x.AudienceLevel);
			Map(x => x.Room);
			Map(x => x.Time);
			Map(x => x.SpeakerRateUrl).Length(2048);
			References(x => x.Speaker);
			References(x => x.Track);
		}
	}
}