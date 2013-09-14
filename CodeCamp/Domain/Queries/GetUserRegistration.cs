using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;

namespace CodeCamp.Domain.Queries {
    public class GetUserRegistration : Query<Registration> {
        readonly string eventId;
        readonly string userId;

        public GetUserRegistration(string eventId, string userId) {
            this.eventId = eventId;
            this.userId = userId;
        }

        protected override Registration Execute() {
            return DocSession.Load<Registration>(
                Registration.IdFrom(
                    Event.SlugFromId(eventId),
                    User.SlugFromId(userId)
                    )
                );
        }
    }
}