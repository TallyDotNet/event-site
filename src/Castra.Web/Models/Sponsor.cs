namespace Castra.Web.Models
{
    using System;
    using BlueSpire.Kernel.Data;
    using BlueSpire.NHibernate;

    public class Sponsor : Entity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Blurb { get; set; }
        public virtual string DisplayUrl { get; set; }
        public virtual string Url { get; set; }
        public virtual string BannerUrl { get; set; }
        public virtual SponsorLevel Level { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int SortOrder { get; set; }

		public override string ToString()
		{
			return Name;
		}
    }

    public class SponsorMap : EntityMap<Sponsor>
    {
        public SponsorMap()
        {
            Map(x => x.Name);
            Map(x => x.Blurb).Length(8000);
            Map(x => x.DisplayUrl).Length(2048);
            Map(x => x.Url).Length(2048);
            Map(x => x.BannerUrl).Length(2048);
            Map(x => x.Level);
			Map(x => x.IsActive);
			Map(x => x.SortOrder);
        }
    }
}