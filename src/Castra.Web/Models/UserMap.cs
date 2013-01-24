namespace Castra.Web.Models
{
	using BlueSpire.Web.Mvc.Membership;
	using BlueSpire.NHibernate;

	public class UserMap : EntityMap<User>
	{
		public UserMap()
		{
			Map(x => x.Username);
			Map(x => x.Password);
			Map(x => x.MemberSince);
			Map(x => x.LastLogin);
			Map(x => x.RegistrationIPAddress);
			Map(x => x.Role);
			Map(x => x.Status);
		}
	}
}