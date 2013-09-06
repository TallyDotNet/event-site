using System.Web.Mvc;
using CodeCamp.Domain;

namespace CodeCamp.Infrastructure.Views {
    public abstract class BaseView<TModel> : WebViewPage<TModel> {
        public ViewInfo Info { get; private set; }
        public IApplicationState State { get; private set; }

        protected BaseView() {
            Info = DependencyResolver.Current.GetService<ViewInfo>();
            State = DependencyResolver.Current.GetService<IApplicationState>();
        }
    }
}