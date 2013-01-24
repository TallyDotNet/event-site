namespace Castra.Web.Queries
{
    using System.Linq;
    using BlueSpire.Web.Mvc.Membership;
    using BlueSpire.NHibernate;
    using BlueSpire.NHibernate.QueryHandlers;
    using Models;

    public class IsUsernameAvailableHandler : NotFoundHandler<IsUsernameAvailable, User>
    {
        public IsUsernameAvailableHandler(ISessionSource sessionSource)
            : base(sessionSource)
        {
        }

        protected override IQueryable<User> CreateQuery(IsUsernameAvailable query, IQueryable<User> users)
        {
            return from user in users
                   where user.Username == query.Username
                   select user;
        }
    }
}