using CodeCamp.Domain.Infrastructure;

namespace CodeCamp.Domain.Commands {
    public class CreateAccount : Command<CommandResponse> {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        protected override CommandResponse Execute() {
            return CommandResponse.SuccessMessage("Your account has been successfully created.");
        }
    }
}