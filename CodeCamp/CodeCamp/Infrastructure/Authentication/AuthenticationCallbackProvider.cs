using System.Web;
using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Queries;
using CodeCamp.ViewModels;
using CodeCamp.ViewModels.Account;
using SimpleAuthentication.Mvc;

namespace CodeCamp.Infrastructure.Authentication {
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

            var user = bus.Query(new UserViaProvider(authInfo.ProviderName, userInfo.Id));
            if(user != null) {
                state.Login(user, true);
                return new RedirectResult(model.ReturnUrl);
            }

            if(!string.IsNullOrEmpty(userInfo.Email)) {
                user = bus.Query(new UserWithEmail(userInfo.Email));
                if(user != null) {
                    user.AddOAuthAccount(authInfo.ProviderName, userInfo.Id);
                    state.Login(user, true);
                    return new RedirectResult(model.ReturnUrl);
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
    }
}