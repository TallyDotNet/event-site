using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using EventSite.Domain;
using EventSite.Domain.Infrastructure;
using EventSite.Infrastructure.Authentication;
using EventSite.Infrastructure.Data;
using EventSite.Infrastructure.Logging;
using EventSite.Infrastructure.Views;
using Raven.Client;
using SimpleAuthentication.Mvc;

namespace EventSite.Infrastructure.IoC {
    public class ContainerConfig {
        public static void Configure() {
            var builder = new ContainerBuilder();
            var settings = new WebConfigSettings();

            RegisterMVCComponents(builder);
            RegisterApplicationComponents(builder, settings);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));    
        }

        static void RegisterApplicationComponents(ContainerBuilder builder, WebConfigSettings settings) {
            builder.RegisterModule<NLogModule>();
            
            builder.RegisterType<ViewInfo>().InstancePerHttpRequest();
            
            builder.RegisterType<SingleWebServerApplicationState>().As<IApplicationState>().InstancePerHttpRequest();
            builder.RegisterType<DefaultApplicationBus>().As<IApplicationBus>().InstancePerHttpRequest();
            builder.RegisterInstance(settings).As<ISettings>().SingleInstance();
            
            builder.Register(x => RavenDBConfig.CreateDocumentStore()).As<IDocumentStore>().SingleInstance();
            builder.Register(x => x.Resolve<IDocumentStore>().OpenSession()).As<IDocumentSession>().InstancePerHttpRequest();
            
            builder.RegisterType<AuthenticationCallbackProvider>().As<IAuthenticationCallbackProvider>();
            builder.RegisterType<SimpleAuthenticationController>().As<SimpleAuthenticationController>().InstancePerHttpRequest();
            
            builder.RegisterType<DefaultSecurityEncoder>().As<ISecurityEncoder>().SingleInstance();
            builder.RegisterType<DefaultSlugConverter>().As<ISlugConverter>().SingleInstance();

            if(settings.RunningInProduction()) {
                builder.RegisterType<CloudinaryImageStorage>().As<IImageStorage>().SingleInstance();
            } else {
                builder.RegisterType<LocalImageStorage>().As<IImageStorage>().SingleInstance();
            }

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