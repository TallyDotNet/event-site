using EventSite.Domain;
using EventSite.Domain.Commands;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure;
using Raven.Client;
using Raven.Client.Document;
using Rhino.Mocks;
using Shouldly;
using SpecEasy;

namespace EventSite.Tests.Domain.Commands
{
    public class CreateAccountSpecs : Spec<CreateAccount> {
        private const string ExternalLoginData = "test_external_login_data";
        public void Test() {
            
            string dummyProviderName;
            string dummyProviderUserId;
            Result<User> result = null;
            When("invoking the \"Create Account\" command", () => result = SUT.Process());

            Given(() => SetupMocks()).Verify(() =>
            {
                Given("all fields required to create a user have been provied", () => SetRequiredFields()).Verify(() =>
                {
                    Given("the external login data cannot be converted to an oauth user id", () =>
                        SUT.SecurityEncoder.Stub(e => e.TryDeserializeOAuthProviderUserId(
                            ExternalLoginData, out dummyProviderName, out dummyProviderUserId)).Return(false)).Verify(() =>
                            {
                                Then("the command should return a failure status.", () => result.Status.ShouldBe(ResultStatus.Failure));
                                Then("the error message reutnred by the command should explain that the login data wasn't valid.", () => result.Message.ShouldContain("Invalid external login data provided."));
                            });

                    Given("the external login data can be converted to an oath user id", () =>
                    {
                        SUT.SecurityEncoder.Stub( e => e.TryDeserializeOAuthProviderUserId(ExternalLoginData, out dummyProviderName, out dummyProviderUserId)).Return(true).Repeat.Any();
                        SUT.SlugConverter.Stub(e => e.ToSlug(SUT.Username)).Return(SUT.Username + "_slug").Repeat.Any();
                       
                    }).Verify(() =>
                        {
                            Then("it should convert the user name to a slug to be stored in the event site database.", () => SUT.SlugConverter.AssertWasCalled(c => c.ToSlug(Arg<string>.Is.Anything)));
                            
                            Given("the username does not yet exist in the database.", () => SUT.DocSession.Stub(d => d.Load<User>(Arg<string>.Is.Anything)).Return(null)).Verify(() =>
                                {
                                    Given("the email does not yet exist in the database.", () => SUT.Bus.Stub(b => b.Query(Arg<UserWithEmail>.Is.Anything)).Return(null)).Verify(() =>
                                        {
                                            Then("it should store the user in the database", () => SUT.DocSession.AssertWasCalled(d => d.Store(Arg<User>.Is.Anything))); 
                                            Then("it should return a success result.", () => result.Succeeded().ShouldBe(true));
                                        });
                                });
                        });
                });
            });            
        }

        private void SetupMocks() {
            SUT.SecurityEncoder = Mock<ISecurityEncoder>();
            SUT.SlugConverter = Mock<ISlugConverter>();
            SUT.DocSession = Mock<IDocumentSession>();
            SUT.Bus = Mock<IApplicationBus>();
            SUT.State = Mock<IApplicationState>();
            SUT.State.Stub(s => s.Settings).Return(new WebConfigSettings()).Repeat.Any();
        }

        private void SetRequiredFields() {
            SUT.Username = "testUser";
            SUT.Email = "testUser@gmail.com";
            SUT.ExternalLoginData = ExternalLoginData;
        }
    }
}
