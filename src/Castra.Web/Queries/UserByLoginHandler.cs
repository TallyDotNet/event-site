namespace Castra.Web.Queries
{
    using System.Linq;
    using BlueSpire.Web.Mvc.Membership;
    using BlueSpire.NHibernate;
    using BlueSpire.NHibernate.QueryHandlers;
    using Models;

    public class UserByLoginHandler : ValueQueryHandler<UserByLogin, User>
    {
        public UserByLoginHandler(ISessionSource sessionSource)
            : base(sessionSource) { }

        protected override IQueryable<User> CreateQuery(UserByLogin query, IQueryable<User> users)
        {
            return from user in users
                   where user.Username == query.Username
                         && user.Password == query.HashedPassword 
                   select user;
        }
    }
}