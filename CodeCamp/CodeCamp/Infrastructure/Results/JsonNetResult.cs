using System;
using System.Web.Mvc;
using CodeCamp.Infrastructure.Data;
using Raven.Imports.Newtonsoft.Json;

namespace CodeCamp.Infrastructure.Results {
    public class JsonNetResult : ActionResult {
        readonly object data;

        public JsonNetResult(object data) {
            this.data = data;
        }

        public override void ExecuteResult(ControllerContext context) {
            if(context == null) {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = "application/json";

            if(data == null) {
                return;
            }

            var writer = new JsonTextWriter(response.Output);

            var serializer = new JsonSerializer();
            serializer.Configure();
            serializer.Serialize(writer, data);
            writer.Flush();
        }
    }
}