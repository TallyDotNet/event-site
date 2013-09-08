using System;
using System.Web;
using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Queries;
using CodeCamp.ViewModels.Account;
using SimpleAuthentication.Mvc;

namespace CodeCamp.Infrastructure.Authentication {
    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider {
        readonly IApplicationState state;
        readonly IApplicationBus bus;

        public AuthenticationCallbackProvider(IApplicationState state, IApplicationBus bus) {
            this.state = state;
            this.bus = bus;
        }

        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model) {
            var userInfo = model.AuthenticatedClient.UserInformation;
            var user = bus.Query(new UserWithEmail(userInfo.Email));

            if(user != null) {
                state.Login(user, true);
                return new RedirectResult(model.ReturnUrl);
            }

            return new ViewResult {
                ViewName = "Create",
                ViewData = new ViewDataDictionary(new CreateAccountViewModel {
                    Email = userInfo.Email,
                    Username = userInfo.UserName,
                    ReturnUrl = model.ReturnUrl
                })
            };
        }

        public ActionResult OnRedirectToAuthenticationProviderError(HttpContextBase context, string errorMessage) {
            throw new NotImplementedException();
        }
    }
}