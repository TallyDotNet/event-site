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
            return DocSession.Query<User, Index>()
                .FirstOrDefault(x => x.Email == email);
        }

        public class Index : AbstractIndexCreationTask<User> {
            public Index() {
                Map = users =>
                    from user in users
                    select user.Email;
            }
        }
    }
}