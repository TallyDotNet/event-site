namespace Castra.Web.Commands
{
    using BlueSpire.Kernel.Bus;
    using Models;

	public class ProposeSession : ICommand<Result>
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
		public AudienceLevel AudienceLevel { get; set; }
    }
}