namespace Castra.Web.Models
{
    using System;
    using BlueSpire.Kernel.Data;
    using BlueSpire.NHibernate;

    public class Conference : Entity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Location { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

		public override string ToString()
		{
			return Name;
		}
    }

    public class ConferenceMap : EntityMap<Conference>
    {
        public ConferenceMap()
        {
            Map(x => x.Name);
            Map(x => x.Location);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
        }
    }
}