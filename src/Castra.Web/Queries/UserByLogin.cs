namespace Castra.Web.Queries
{
    using BlueSpire.Kernel.Data;
    using BlueSpire.Web.Mvc.Membership;
    using Models;

    public class UserByLogin : IQuery<User>
    {
        public string Username { get; private set; }
        public string HashedPassword { get; private set; }

        public UserByLogin(string username, string hashedPassword)
        {
            Username = username.ToLower();
            HashedPassword = hashedPassword;
        }
    }
}