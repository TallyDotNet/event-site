using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using Raven.Client.Indexes;

namespace CodeCamp.Domain.Queries {
    public class UserViaProvider : Query<User> {
        readonly string provider;
        readonly string providerUserId;

        public UserViaProvider(string provider, string providerUserId) {
            this.provider = provider;
            this.providerUserId = providerUserId;
        }

        protected override User Execute() {
            return DocSession.Query<User>()
                .SingleOrDefault(u => u.OAuthAccounts.Any(r => r.Provider == provider && r.ProviderUserId == providerUserId));
        }

        public class Index : AbstractIndexCreationTask<User> {
            public Index() {
                Map = users =>
                    from user in users
                    select new {user.OAuthAccounts};
            }
        }
    }
}