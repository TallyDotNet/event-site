using System.Reflection;
using NLog;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace CodeCamp.Infrastructure.Data {
    public static class RavenDBConfig {
        static readonly Logger Log = LogManager.GetLogger(typeof(RavenDBConfig).FullName);

        public static IDocumentStore CreateDocumentStore(bool createIndexes = true) {
            Log.Info("Initializing RavenDB.");

            var parser = ConnectionStringParser<RavenConnectionStringOptions>
                .FromConnectionStringName("RavenDB");

            parser.Parse();

            var store = new DocumentStore {
                ApiKey = parser.ConnectionStringOptions.ApiKey,
                Url = parser.ConnectionStringOptions.Url,
                Conventions = {
                    CustomizeJsonSerializer = serializer => serializer.Configure(),
                }
            };

            store.Initialize();

            if(createIndexes) {
                IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), store);
            }

            Log.Info("RavenDB intialized.");

            return store;
        }
    }
}