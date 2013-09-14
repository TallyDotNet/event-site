using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client.Indexes;

namespace EventSite.Domain.Queries {
    public class UserViaProvider : Query<User> {
        readonly string provider;
        readonly string providerUserId;

        public UserViaProvider(string provider, string providerUserId) {
            this.provider = provider;
            this.providerUserId = providerUserId;
        }

        protected override User Execute() {
            return DocSession.Query<User, UserViaProviderIndex>()
                .SingleOrDefault(u => u.AuthAccounts.Any(r => r.Provider == provider && r.ProviderUserId == providerUserId));
        }

        public class UserViaProviderIndex : AbstractIndexCreationTask<User> {
            public UserViaProviderIndex() {
                Map = users =>
                    from user in users
                    from account in user.AuthAccounts
                    select new {
                        AuthAccounts_ProviderUserId = account.ProviderUserId,
                        AuthAccounts_Provider = account.Provider
                    };
            }
        }
    }
}