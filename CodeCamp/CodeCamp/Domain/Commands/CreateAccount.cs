using System.ComponentModel.DataAnnotations;
using CodeCamp.Domain.Infrastructure;

namespace CodeCamp.Domain.Commands {
    public class CreateAccount : Command<CommandResponse> {
        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool Persist { get; set; }

        protected override CommandResponse Execute() {
            return CommandResponse.SuccessMessage("Your account has been successfully created.");
        }
    }
}