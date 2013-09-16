﻿using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Infrastructure;
using Raven.Imports.Newtonsoft.Json;

namespace EventSite.Infrastructure.Views {
    public abstract class BaseView<TModel> : WebViewPage<TModel> {
        public ViewInfo ViewInfo { get; private set; }
        public IApplicationState State { get; private set; }

        Result result;
        public Result Result {
            get {
                if(result != null) {
                    return result;
                }

                var message = TempData["Result"];
                if(message != null) {
                    result = JsonConvert.DeserializeObject<Result>(message.ToString());
                }

                return result;
            }
        }

        protected BaseView() {
            ViewInfo = DependencyResolver.Current.GetService<ViewInfo>();
            State = DependencyResolver.Current.GetService<IApplicationState>();
        }
    }
}