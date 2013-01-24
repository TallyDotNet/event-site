namespace Castra.Web
{
	using System;
	using System.Reflection;
	using System.Web.Mvc;
	using System.Web.Routing;
	using BlueSpire.Kernel;
	using BlueSpire.Kernel.Data;
	using BlueSpire.NHibernate;
	using BlueSpire.Web.Mvc.Infrastructure;
	using BlueSpire.Web.Mvc.Membership;
	using Caliburn.Core;
	using Caliburn.Core.Configuration;
	using Caliburn.FluentValidation;
	using Caliburn.Ninject;
	using Configuration;
	using Controllers;
	using FluentNHibernate.Cfg.Db;
	using log4net;
	using log4net.Config;
	using Models;
	using NHaml.Web.Mvc;
	using NHibernate;
	using Ninject;
	using Ninject.Web.Mvc;
    using HibernatingRhinos.Profiler.Appender.NHibernate;

	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : NinjectHttpApplication
	{
		static readonly ILog Log = LogManager.GetLogger(typeof (MvcApplication));

		protected MvcApplication()
		{
#if DEBUG
			NHibernateProfiler.Initialize();
#endif
			EndRequest += MvcApplication_EndRequest;
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("favicon.ico");

			routes.AddRoute("resource/{id}", "Resource", "Index");
			routes.AddRoute("speakers/details/{lookupName}", "Speakers", "Details");
			routes.AddRoute("session/propose", "Session", "Propose");
			routes.AddRoute("session/proposals", "Session", "Proposals");
			routes.AddRoute("session/{lookupName}", "Session", "Details");
			routes.AddRoute("sessions/by/{groupBy}", "Session", "Index");
			routes.AddRoute("session/approve/{proposalId}", "Session", "Approve");
			routes.AddRoute("speakers/{page}", "Speakers", "Index");

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new
					{
						controller = "Home",
						action = "Index",
						id = ""
					}, new[] {"Castra.Web.Controllers"}
				);
		}

		void MvcApplication_EndRequest(object sender, EventArgs e)
		{
			if (UnitOfWork.IsStarted)
				UnitOfWork.DisposeUnitOfWork(UnitOfWork.Current);
		}

		void Application_Error(object sender, EventArgs e)
		{
			var exception = Server.GetLastError();

			if (exception.GetType() == typeof (UnauthorizedAccessException))
			{
				var user = Session["User"] as UserSummary;
				var username = (user == null) ? "[guest]" : user.Username;

				var page = Request.RawUrl;
				Log.Error(string.Format("The user {0} attempted to access {1}", username, page));

				Server.ClearError();
				Response.Redirect("~/Error/NotAuthorized");
			}
			else
			{
				Log.Error("Unhandled Exception", exception);
				Server.ClearError();
				Response.Redirect("~/Error/Unknown");
			}
		}

		protected override void OnApplicationStarted()
		{
			XmlConfigurator.Configure();
			Log.Info("Web App Starting");

			EnumerationBinderFor<AudienceLevel>();
			EnumerationBinderFor<ContentType>();
			EnumerationBinderFor<ShirtSize>();
			EnumerationBinderFor<SponsorLevel>();

			AreaRegistration.RegisterAllAreas();

			RegisterMainMenuRoutes(RouteTable.Routes);
			RegisterRoutes(RouteTable.Routes);

			ViewEngines.Engines.Add(new NHamlMvcViewEngine());
			RegisterAllControllersIn(Assembly.GetExecutingAssembly());

			Kernel.Bind<IUnitOfWorkFactory>().ToConstant(CreateSessionFactory());

			var source = new SessionSource();
			Kernel.Bind<ISessionSource>().ToConstant(source);
			Kernel.Bind<ISession>().ToMethod(ctx => source.GetSession());
		}

		static void EnumerationBinderFor<T>() where T : Enumeration
		{
			ModelBinders.Binders[typeof (T)] = new EnumerationModelBinder<T>();
		}

		void RegisterMainMenuRoutes(RouteCollection routes)
		{
			var menu = Kernel.Get<MainMenu>();
			foreach (var entry in menu.Items)
			{
				routes.MapRoute(
					entry.Label,
					entry.Label,
					new {controller = entry.Controller, action = entry.Action},
					new[] {"Castra.Web.Controllers"}
					);
			}
		}

		protected override IKernel CreateKernel()
		{
			var kernel = new StandardKernel(new GeneralRegistration(Assembly.GetExecutingAssembly()));

			CaliburnFramework.Configure(new NinjectAdapter(kernel))
				.With.Core()
				.Using(x => x.Validator<FluentValidationValidator>())
				.Start();

			return kernel;
		}

		static IUnitOfWorkFactory CreateSessionFactory()
		{
			var factory = new NhUnitOfWorkFactory();

			var dbConfig = MsSqlConfiguration
				.MsSql2008
				.ConnectionString(ConnectionString.ForMachine());

			factory.Initialize(config => config
			                             	.Database(dbConfig)
			                             	.Mappings(mapper => mapper.FluentMappings
			                             	                    	.AddFromAssemblyOf<MvcApplication>()
			                             	                    	.Conventions.Add<EscapeColumnNamesConvention>()));

			return factory;
		}
	}
}