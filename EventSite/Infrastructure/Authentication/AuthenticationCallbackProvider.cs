using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventSite.Domain;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Routing;
using EventSite.ViewModels;
using EventSite.ViewModels.Account;
using SimpleAuthentication.Mvc;

namespace EventSite.Infrastructure.Authentication {
    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider {
        readonly IApplicationState state;
        readonly IApplicationBus bus;
        readonly ISecurityEncoder securityEncoder;

        public AuthenticationCallbackProvider(IApplicationState state, IApplicationBus bus, ISecurityEncoder securityEncoder) {
            this.state = state;
            this.bus = bus;
            this.securityEncoder = securityEncoder;
        }

        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model) {
            var authInfo = model.AuthenticatedClient;
            var userInfo = authInfo.UserInformation;
            var returnUrl = RouteHelper.GetReturnUrl(model.ReturnUrl);

            var user = bus.Query(new UserViaProvider(authInfo.ProviderName, userInfo.Id));
            if(user != null) {
                state.Login(user, true);

                if(state.RegistrationStatus == RegistrationStatus.NotRegistered) {
                    return RedirectToAction("Registration", "Create");
                }

                return !string.IsNullOrEmpty(returnUrl)
                    ? new RedirectResult(returnUrl)
                    : RedirectToAction("Account");
            }

            if(!string.IsNullOrEmpty(userInfo.Email)) {
                user = bus.Query(new UserWithEmail(userInfo.Email));

                if(user != null) {
                    user.AddOAuthAccount(authInfo.ProviderName, userInfo.Id);

                    state.Login(user, true);

                    if(state.RegistrationStatus == RegistrationStatus.NotRegistered) {
                        return RedirectToAction("Registration", "Create");
                    }

                    return !string.IsNullOrEmpty(returnUrl)
                        ? new RedirectResult(returnUrl)
                        : RedirectToAction("Account");
                }
            }

            return new ViewResult {
                ViewName = "~/Views/Account/Create.cshtml",
                ViewData = new ViewDataDictionary(new CreateAccountViewModel {
                    Email = userInfo.Email,
                    Username = userInfo.UserName,
                    ReturnUrl = model.ReturnUrl,
                    ExternalLoginData = securityEncoder.SerializeOAuthProviderUserId(authInfo.ProviderName, userInfo.Id),
                    Persist = true,
                    ProviderDisplayName = authInfo.ProviderName
                })
            };
        }

        public ActionResult OnRedirectToAuthenticationProviderError(HttpContextBase context, string errorMessage) {
            return new ViewResult {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(new ErrorOutput {
                    ErrorCode = "500",
                    Message = errorMessage,
                    Url = context.Request.Url.OriginalString
                })
            };
        }

        ActionResult RedirectToAction(string controller, string action = "Index") {
            var values = new RouteValueDictionary();

            values["controller"] = controller;
            values["action"] = action;

            return new RedirectToRouteResult(values);
        }
    }
}