namespace Castra.Web.Commands
{
    using BlueSpire.Kernel.Bus;
    using Models;

	public class EditProfile : ICommand<Result>
    {
        public string Name { get; set; }
        public string BlogUrl { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string CompanyUrl { get; set; }
        public bool IsMVP { get; set; }
        public bool IsPublic { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string Twitter { get; set; }
        public string Bio { get; set; }
		public ShirtSize ShirtSize { get; set; }
		public bool OptOut { get; set; }
    }
}