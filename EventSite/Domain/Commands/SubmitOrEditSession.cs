using System.ComponentModel.DataAnnotations;
using System.Web.WebPages;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;

namespace EventSite.Domain.Commands {
    public class SubmitOrEditSession : Command<Result> {
        public string SessionSlug { get; set; }
        public string EventSlug { get; set; }
        public string UserSlug { get; set; }

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
            if(!State.UserIsAdmin() && !State.RegisteredForEvent()) {
                return Error("You are not registered for the event. Please register before submitting a session.");
            }

            if (!State.UserIsAdmin() && (!State.EventScheduled() || !State.CurrentEvent.IsSessionSubmissionOpen)) {
                return Error("The currently scheduled event is not currently open for session submission.");
            }

            var sessionSlug = SlugConverter.ToSlug(Name);
            var sessionId = Session.IdFrom(State.CurrentEventSlug(), sessionSlug);

            if(DocSession.Load<Session>(sessionId) != null) {
                return Error("The provided session name is not available.");
            }

            Reference sessionUser;
            if (State.UserIsAdmin() && !UserSlug.IsEmpty()) {
                var targetUser = DocSession.Load<User>(User.IdFrom(UserSlug));
                if (targetUser == null) {
                    return Error(string.Format("Unable to find user with slug '{0}'", UserSlug));
                }

                sessionUser = new Reference {
                    Id = targetUser.Id,
                    Name = targetUser.Username
                };
            }
            else {
                sessionUser = new Reference {
                    Id = CurrentUser.Id,
                    Name = CurrentUser.Username
                };
            }

            var session = new Session {
                Id = sessionId,
                Name = Name,
                Description = Description,
                Level = Level,
                Event = new Reference {
                    Id = State.CurrentEvent.Id,
                    Name = State.CurrentEvent.Name
                },
                User = sessionUser,
                SubmittedOn = Now(),
                Status = SessionStatus.PendingApproval
            };

            DocSession.Store(session);
            return SuccessFormat("You have successfully submitted \"{0}\" to {1}.", Name, State.CurrentEvent.Name);
        }
    }
}