using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using Raven.Client.Indexes;

namespace CodeCamp.Domain.Queries {
    public class UserWithUsername : Query<User> {
        readonly string username;

        public UserWithUsername(string username) {
            this.username = username;
        }

        protected override User Execute() {
            return DocSession.Query<User, Index>()
                .SingleOrDefault(x => x.Username == username);
        }

        public class Index : AbstractIndexCreationTask<User> {
            public Index() {
                Map = users =>
                    from user in users
                    select new {user.Username};
            }
        }
    }
}