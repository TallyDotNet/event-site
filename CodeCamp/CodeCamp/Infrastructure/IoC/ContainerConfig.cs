using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using CodeCamp.Domain;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Infrastructure.Logging;
using CodeCamp.Infrastructure.Models;

namespace CodeCamp.Infrastructure.IoC {
    public class ContainerConfig {
        public static void Configure() {
            var builder = new ContainerBuilder();

            RegisterMVCComponents(builder);
            RegisterApplicationComponents(builder);

            //Plug into MVC
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));    
        }

        static void RegisterApplicationComponents(ContainerBuilder builder) {
            builder.RegisterModule<NLogModule>();
            builder.RegisterType<PageInfo>().InstancePerHttpRequest();
            builder.RegisterType<SingleWebServerApplicationState>().As<IApplicationState>().InstancePerHttpRequest();
            builder.RegisterType<DefaultApplicationBus>().As<IApplicationBus>().SingleInstance();
        }

        static void RegisterMVCComponents(ContainerBuilder builder) {
            //Register Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //Register Model Binders
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            //Register Standard HTTP Abstractions
            builder.RegisterModule(new AutofacWebTypesModule());

            //Enable property injection for Views
            builder.RegisterSource(new ViewRegistrationSource());

            //Enable property injection for Filters
            builder.RegisterFilterProvider();
        }
    }
}