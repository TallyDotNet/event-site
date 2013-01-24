namespace Castra.Web.Commands
{
	using BlueSpire.Kernel.Bus;

	public class ChangePassword : ICommand<Result>
	{
		public string NewPassword { get; set;}
		public string PasswordVerification { get; set; }
	}
}