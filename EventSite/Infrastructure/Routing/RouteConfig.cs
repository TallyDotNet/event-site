using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using EventSite.Controllers;

namespace EventSite.Infrastructure.Routing {
    public class RouteConfig {
        readonly RouteCollection routes;

        public RouteConfig(RouteCollection routes) {
            this.routes = routes;
        }

        public void Configure() {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new {favicon = @"(.*/)?favicon.ico(/.*)?"});

            RouteFor("home")
                .HandledBy<HomeController>(x => x.Index());

            RouteFor("account")
                .HandledBy<AccountController>(x => x.Index());

            RouteFor("account/login")
                .HandledBy<AccountController>(x => x.Login());

            RouteFor("account/new")
                .HandledBy<AccountController>(x => x.Create(null));

            RouteFor("account/logout")
                .HandledBy<AccountController>(x => x.Logout());

            RouteFor("events/{eventSlug}/sessions/{sessionSlug}/approve")
                .HandledBy<SessionsController>(x => x.Approve(null, null));

            RouteFor("events/{eventSlug}/sessions/{sessionSlug}/reject")
                .HandledBy<SessionsController>(x => x.Reject(null, null));

            RouteFor("events/{eventSlug}/sessions/new")
                .HandledBy<SessionsController>(x => x.Create());

            RouteFor("events/{eventSlug}/sessions")
                .HandledBy<SessionsController>(x => x.Index(null, 0, null));

            RouteFor("sessions")
                .HandledBy<SessionsController>(x => x.Index(null, 0, null));

            RouteFor("events/{eventSlug}/registrations/new")
                .HandledBy<RegistrationController>(x => x.Create());

            RouteFor("events/{eventSlug}/registrations")
                .HandledBy<RegistrationController>(x => x.Index(null));

            RouteFor("registrations/new")
                .HandledBy<RegistrationController>(x => x.Create());

            RouteFor("events/{eventSlug}/make-current")
                .HandledBy<EventsController>(x => x.MakeCurrent(null));

            RouteFor("events/{eventSlug}/location")
                .HandledBy<LocationController>(x => x.Index(null));

            RouteFor("location")
                .HandledBy<LocationController>(x => x.Index(null));

            RouteFor("events/{eventSlug}/speakers/{speakerSlug}")
                .HandledBy<SpeakersController>(x => x.Index(null, null));

            RouteFor("events/{eventSlug}/speakers")
                .HandledBy<SpeakersController>(x => x.Index(null, null));

            RouteFor("speakers")
                .HandledBy<SpeakersController>(x => x.Index(null, null));

            RouteFor("events/{eventSlug}/sponsors")
                .HandledBy<SponsorsController>(x => x.Index());

            RouteFor("sponsors")
                .HandledBy<SponsorsController>(x => x.Index());

            RouteFor("events/new")
                .HandledBy<EventsController>(x => x.Create());

            RouteFor("events/{eventSlug}")
                .HandledBy<EventsController>(x => x.Detail(null));

            RouteFor("events")
                .HandledBy<EventsController>(x => x.Index());

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            routes.MapRoute(
                "NotFound",
                "{*url}",
                new {controller = "Error", action = "Error", errorCode = 404, message = "Not Found"}
                );
        }

        RouteBuilder RouteFor(string route) {
            return new RouteBuilder(routes, route);
        }
    }
}