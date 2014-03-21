using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSite.Domain.Commands;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Rhino.Mocks;
using SpecEasy;
using Action = SpecEasy.Action;
using Shouldly;


namespace EventSite.Tests
{
    public class EventSiteCommandSpec<TCommand, TResult> : Spec<TCommand>
        where TCommand: Command<TResult> where TResult : Result, new() {

        protected void WhenExecutingCommand(string description) {
           
        }
    }

    public class CreateAccountSpec : Spec<CreateAccount>
    {
        public void Test() {

            string dummyProviderName;
            string dummyProviderUserId;
            Result<User> result = null;
            When("creating a new account", () => result = SUT.Process());

            const string externalLoginData = "I'm a user!";

            Given(() => SUT.ExternalLoginData = externalLoginData).Verify(() =>
            {
            Given("the external login data cannot be converted to an oauth user id", () => 
                Get<ISecurityEncoder>().Stub(e => e.TryDeserializeOAuthProviderUserId(externalLoginData, out dummyProviderName, out dummyProviderUserId))).Verify(() =>
                    {
                        Then("the command should return a failure status.", () => result.Status.ShouldBe(ResultStatus.Failure));
                        Then("the error message reutnred by the command should explain that the login data wasn't valid.", () => result.Message.ShouldContain("Invalid external login data provided."));

                    });    
            });
            
        }
    }
}
