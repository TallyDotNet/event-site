namespace Castra.Web.Models
{
	using System;
	using BlueSpire.Kernel.Data;
	using BlueSpire.NHibernate;
	using BlueSpire.Web.Mvc.Infrastructure;

	public class Track : Entity<Guid>
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

		public override string ToString()
		{
			return Title;
		}
	}

	public class TrackMap : EntityMap<Track>
	{
		public TrackMap()
		{
			Map(x => x.Title);
			Map(x => x.LookupName);
			Map(x => x.Abstract).Length(8000);
		}
	}
}