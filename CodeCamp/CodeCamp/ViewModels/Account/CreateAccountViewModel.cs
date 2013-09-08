using CodeCamp.Domain.Commands;

namespace CodeCamp.ViewModels.Account {
    public class CreateAccountViewModel : CreateAccount {
        public string ReturnUrl { get; set; }
    }
}