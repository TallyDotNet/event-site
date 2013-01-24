namespace Castra.Web.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using BlueSpire.Kernel.Bus;
    using BlueSpire.Kernel.Data;
    using BlueSpire.Web.Mvc.Infrastructure;
    using BlueSpire.NHibernate;
    using BlueSpire.NHibernate.QueryHandlers;
    using BlueSpire.Web.Mvc.Membership;
    using Caliburn.Core.IoC;
    using FluentValidation;
    using Ninject.Modules;

    public class GeneralRegistration : NinjectModule
    {
        private readonly Assembly assembly;

        public GeneralRegistration(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public override void Load()
        {
            Bind<IDataSource>().To<NhRepository>().InTransientScope();
            Bind<IRepository>().To<NhRepository>().InTransientScope();
            Bind<IApplicationBus<IContextProvider>>().To<SimpleApplicationBus<IContextProvider>>().InTransientScope();
            Bind<IContextProvider>().To<WebContextProvider>().InSingletonScope();

            RegisterOpenTypes(typeof(IQueryHandler<>));
            RegisterIdHandlers();
            RegisterOpenTypes(typeof(ICommandHandler<>));
            RegisterOpenTypes(typeof(IMessageHandler<>));
            RegisterOpenTypes(typeof(IValidator<>));
        }

        private void RegisterIdHandlers()
        {
        	var types = assembly.GetExportedTypes().Union( new[]{ typeof(User)});

			var matches = from type in types
                          where typeof(IEntity).IsAssignableFrom(type) &&
                              !type.IsAbstract && !type.IsInterface
                          select new {
                              Service = typeof(IQueryHandler<>).MakeGenericType(typeof(QueryById<>).MakeGenericType(type)),
                              Implementation = typeof(IdQueryHandler<>).MakeGenericType(type),
                          };

            foreach (var match in matches)
            {
                Bind(match.Service).To(match.Implementation).InTransientScope();
            }
        }

        private void RegisterOpenTypes(Type openGeneric)
        {
            var matches = from type in assembly.GetExportedTypes()
                          let service = type.FindInterfaceThatCloses(openGeneric)
                          where service != null
                          select new
                          {
                              Service = service,
                              Implementation = type,
                          };

            foreach(var match in matches)
            {
                Bind(match.Service).To(match.Implementation).InTransientScope();
            }
        }
    }
}