namespace Castra.Web.Queries
{
	using BlueSpire.Kernel.Data;

	public class IsUsernameAvailable : IQuery<bool>
    {
        public IsUsernameAvailable(string username)
        {
            Username = username.ToLower();
        }

        public string Username { get; private set; }
    }
}