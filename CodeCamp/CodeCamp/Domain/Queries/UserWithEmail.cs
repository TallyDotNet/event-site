using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using Raven.Client.Indexes;

namespace CodeCamp.Domain.Queries {
    public class UserWithEmail : Query<User> {
        readonly string email;

        public UserWithEmail(string email) {
            this.email = email;
        }

        protected override User Execute() {
            return DocSession.Query<User, UserWithEmailIndex>()
                .SingleOrDefault(x => x.Email == email);
        }

        public class UserWithEmailIndex : AbstractIndexCreationTask<User> {
            public UserWithEmailIndex() {
                Map = users =>
                    from user in users
                    select new {user.Email};
            }
        }
    }
}