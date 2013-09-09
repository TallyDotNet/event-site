using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using CodeCamp.Domain;
using CodeCamp.Domain.Model;
using Raven.Client;

namespace CodeCamp.Infrastructure {
    public class SingleWebServerApplicationState : IApplicationState {
        const string CurrentUserKey = "CurrentUserKey";

        readonly IDocumentSession docSession;
        readonly HttpContextBase httpContext;

        public SingleWebServerApplicationState(IDocumentSession docSession, HttpContextBase httpContext) {
            this.docSession = docSession;
            this.httpContext = httpContext;
        }

        public User User {
            get {
                try {
                    var user = httpContext.Session[CurrentUserKey] as User;
                    if(user != null) {
                        return user;
                    }

                    var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if(authCookie == null || string.IsNullOrWhiteSpace(authCookie.Value)) {
                        return null;
                    }

                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    var userId = authTicket.UserData;

                    if(string.IsNullOrEmpty(userId)) {
                        return null;
                    }

                    user = docSession.Load<User>(userId);

                    if(user != null) {
                        SetUser(user);
                    }

                    return user;
                } catch {
                    return null;
                }
            }
        }

        public Event CurrentEvent { get; private set; }

        public string Environment {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }

        public void Login(User user, bool persist) {
            SetUser(user);
            FormsAuthentication.SetAuthCookie(user.Id, persist);
        }

        public void Logout() {
            httpContext.Session.Clear();
            httpContext.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        void SetUser(User user) {
            httpContext.Session[CurrentUserKey] = user;

            if(httpContext.User != null && httpContext.User.Identity.Name == user.Username) {
                return;
            }

            httpContext.User = new GenericPrincipal(new GenericIdentity(user.Username), user.Roles.ToArray());
        }
    }
}