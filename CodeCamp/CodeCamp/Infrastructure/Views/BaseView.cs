using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Infrastructure.Models;

namespace CodeCamp.Infrastructure.Views {
    public abstract class BaseView<TModel> : WebViewPage<TModel> {
        public PageInfo Info { get; private set; }
        public IApplicationState State { get; private set; }

        protected BaseView() {
            Info = DependencyResolver.Current.GetService<PageInfo>();
            State = DependencyResolver.Current.GetService<IApplicationState>();
        }
    }
}