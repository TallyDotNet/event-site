namespace Castra.Web.Commands
{
    using BlueSpire.Kernel.Bus;
    using BlueSpire.Kernel.Data;
    using FluentValidation;

    public class Login : ICommand<Result>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
		public string ReturnUrl { get; set; }

        public class Validator : AbstractValidator<Login>
        {
            public Validator(IDataSource source)
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
    }
}