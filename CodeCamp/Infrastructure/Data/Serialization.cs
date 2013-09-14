using System.Runtime.Serialization.Formatters;
using Raven.Imports.Newtonsoft.Json;

namespace CodeCamp.Infrastructure.Data {
    public static class Serialization {
        public static void Configure(this JsonSerializer serializer) {
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
            serializer.ObjectCreationHandling = ObjectCreationHandling.Reuse;
        }
    }
}