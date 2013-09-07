using Raven.Client;
using Raven.Client.Document;

namespace CodeCamp.Infrastructure.Data {
    public static class RavenDBConfig {
        public static IDocumentStore CreateDocumentStore() {
            var store = new DocumentStore {
                Url = "http://localhost:8080"
            };

            store.Initialize();

            return store;
        }
    }
}