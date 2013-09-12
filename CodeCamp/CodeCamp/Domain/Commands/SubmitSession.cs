using System.ComponentModel.DataAnnotations;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;

namespace CodeCamp.Domain.Commands {
    public class SubmitSession : Command<Result> {
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        public AudienceLevel Level { get; set; }

        protected override Result Execute() {
            if(!State.RegisteredForEvent()) {
                return Error("You are not registered for the event. Please register before submitting a session.");
            }

            var session = new Session {
                Name = Name,
                Description = Description,
                Level = Level,
                Event = new Reference {
                    Id = State.CurrentEvent.Id,
                    Name = State.CurrentEvent.Name
                },
                User = new Reference {
                    Id = CurrentUser.Id,
                    Name = CurrentUser.Username
                },
                SubmittedOn = Now(),
                Status = SessionStatus.PendingApproval
            };

            DocSession.Store(session);

            return SuccessFormat("You have successfully submitted \"{0}\" to {1}.", Name, State.CurrentEvent.Name);
        }
    }
}