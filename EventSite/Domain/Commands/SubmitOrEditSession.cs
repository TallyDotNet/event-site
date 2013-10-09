using System.ComponentModel.DataAnnotations;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;

namespace EventSite.Domain.Commands {
    public class SubmitOrEditSession : Command<Result> {
        public string SessionSlug { get; set; }
        public string EventSlug { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        public AudienceLevel Level { get; set; }

        public ISlugConverter SlugConverter { get; set; }

        protected override Result Execute() {
            return string.IsNullOrEmpty(SessionSlug)
                ? createNewSession()
                : editExistingSession();
        }

        Result editExistingSession() {
            if(!State.UserIsAdmin()) {
                return Forbidden();
            }

            var session = DocSession.Load<Session>(Session.IdFrom(EventSlug, SessionSlug));
            if(session == null) {
                return NotFound();
            }

            session.Name = Name;
            session.Description = Description;
            session.Level = Level;

            return SuccessFormat("You have successfully edited \"{0}\"", Name);
        }

        Result createNewSession() {
            if(!State.RegisteredForEvent()) {
                return Error("You are not registered for the event. Please register before submitting a session.");
            }

            var slug = SlugConverter.ToSlug(Name);
            var id = Session.IdFrom(State.CurrentEventSlug(), slug);

            if(DocSession.Load<Session>(id) != null) {
                return Error("The provided session name is not available.");
            }

            var session = new Session {
                Id = id,
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