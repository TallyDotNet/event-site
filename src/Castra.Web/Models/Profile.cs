namespace Castra.Web.Models
{
    using System;
    using BlueSpire.Kernel.Data;
    using BlueSpire.Web.Mvc.Membership;
    using BlueSpire.NHibernate;

    public class Profile : Entity<Guid>
    {
        public virtual User Account { get; set; }
        public virtual string Name { get; set; }
        public virtual string LookupName { get; set; }
        public virtual string BlogUrl { get; set; }
        public virtual string Title { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string CompanyUrl { get; set; }
        public virtual bool IsMVP { get; set; }
        public virtual string Location { get; set; }
        public virtual string Phone { get; set; }
		public virtual string Twitter { get; set; }
		public virtual string Bio { get; set; }
		public virtual ShirtSize ShirtSize { get; set; }
		public virtual bool HasApprovedSessions { get; set; }
		public virtual bool IsPublic { get; set; }
		public virtual bool OptOut { get; set; }

		public override string ToString()
		{
			return Name;
		}
    }

    public class ProfileMap : EntityMap<Profile>
    {
        public ProfileMap()
        {
        	Id(x => x.Id).GeneratedBy.Foreign("Account");
            Map(x => x.Name);
            Map(x => x.LookupName);
            Map(x => x.BlogUrl).Length(2048);
            Map(x => x.Title);
            Map(x => x.CompanyName);
            Map(x => x.CompanyUrl).Length(2048);
            Map(x => x.IsMVP);
            Map(x => x.Location);
            Map(x => x.Phone);
            Map(x => x.Twitter);
            Map(x => x.Bio).Length(8000);
        	Map(x => x.ShirtSize);
			Map(x => x.HasApprovedSessions);
			Map(x => x.IsPublic);
			Map(x => x.OptOut);

            References(x => x.Account).Not.LazyLoad();
        }
    }
}