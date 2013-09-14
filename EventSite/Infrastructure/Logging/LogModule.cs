using System;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace EventSite.Infrastructure.Logging {
    public abstract class LogModule<TLogger> : Module {
        protected abstract TLogger CreateLoggerFor(Type type);

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration) {
            var type = registration.Activator.LimitType;
            if(HasPropertyDependencyOnLogger(type)) {
                registration.Activated += InjectLoggerViaProperty;
            }

            if(HasConstructorDependencyOnLogger(type)) {
                registration.Preparing += InjectLoggerViaConstructor;
            }
        }

        bool HasPropertyDependencyOnLogger(Type type) {
            return type.GetProperties().Any(property => property.CanWrite && property.PropertyType == typeof(TLogger));
        }

        bool HasConstructorDependencyOnLogger(Type type) {
            return type.GetConstructors()
                .SelectMany(constructor => constructor.GetParameters()
                    .Where(parameter => parameter.ParameterType == typeof(TLogger)))
                .Any();
        }

        void InjectLoggerViaProperty(object sender, ActivatedEventArgs<object> @event) {
            var type = @event.Instance.GetType();
            var propertyInfo = type.GetProperties().First(x => x.CanWrite && x.PropertyType == typeof(TLogger));
            propertyInfo.SetValue(@event.Instance, CreateLoggerFor(type), null);
        }

        void InjectLoggerViaConstructor(object sender, PreparingEventArgs @event) {
            var type = @event.Component.Activator.LimitType;
            @event.Parameters = @event.Parameters.Union(new[] {
                new ResolvedParameter((parameter, context) => parameter.ParameterType == typeof(TLogger), (p, i) => CreateLoggerFor(type))
            });
        }
    }
}