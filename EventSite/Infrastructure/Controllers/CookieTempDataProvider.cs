//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Web;
//using System.Web.Mvc;

//namespace EventSite.Infrastructure.Controllers {
//    public class CookieTempDataProvider : ITempDataProvider {
//        const string CookieName = "TempData";
//        readonly IFormatter formatter;

//        public CookieTempDataProvider() {
//            formatter = new BinaryFormatter();
//        }

//        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext) {
//            var cookie = controllerContext.HttpContext.Request.Cookies[CookieName];
//            if(cookie != null && !string.IsNullOrEmpty(cookie.Value)) {
//                var bytes = Convert.FromBase64String(cookie.Value);
//                using(var stream = new MemoryStream(bytes)) {
//                    return formatter.Deserialize(stream) as IDictionary<string, object>;
//                }
//            }

//            return new Dictionary<string, object>();
//        }

//        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values) {
//            var cookie = new HttpCookie(CookieName) {HttpOnly = true};

//            if(values.Count == 0) {
//                try {
//                    cookie.Expires = DateTime.Now.AddDays(-1);
//                    cookie.Value = string.Empty;
//                    controllerContext.HttpContext.Response.Cookies.Set(cookie);
//                } catch {
//                    //not important if this fails
//                }

//                return;
//            }

//            using(var stream = new MemoryStream()) {
//                formatter.Serialize(stream, values);
//                var bytes = stream.ToArray();

//                cookie.Value = Convert.ToBase64String(bytes);
//            }

//            controllerContext.HttpContext.Response.Cookies.Add(cookie);
//        }
//    }
//}