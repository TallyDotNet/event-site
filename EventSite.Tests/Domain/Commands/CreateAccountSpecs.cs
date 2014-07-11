using System.Configuration;
using System.Linq;
using EventSite.Domain;
using EventSite.Domain.Commands;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Rhino.Mocks;
using Shouldly;
using SpecEasy;

namespace EventSite.Tests.Domain.Commands
{
    public class CreateAccountSpecs : Spec<CreateAccount> {
        private const string ExternalLoginData = "test_external_login_data";

        private Result<User> result;
        private ISettings settings;

        public void Test() {
            
            string dummyProviderName;
            string dummyProviderUserId;
            string stubbedSlugValue = "testuserslug";

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
                        SUT.SlugConverter.Stub(e => e.ToSlug(SUT.Username)).Return(stubbedSlugValue).Repeat.Any();
                       
                    }).Verify(() =>
                        {
                            Then("it should convert the user name to a slug to be stored in the event site database.", () => SUT.SlugConverter.AssertWasCalled(c => c.ToSlug(Arg<string>.Is.Anything)));
                            
                            Given("the username does not yet exist in the database.", () => SUT.DocSession.Stub(d => d.Load<User>(Arg<string>.Is.Anything)).Return(null)).Verify(() => 
                                Given("the email does not yet exist in the database.", () => SUT.Bus.Stub(b => b.Query(Arg<UserWithEmail>.Is.Anything)).Return(null)).Verify(() =>
                                {
                                    Then("it should store the user in the database", () => SUT.DocSession.AssertWasCalled(d => d.Store(Arg<User>.Is.Anything)));
                                    Then("it should return a success result.", () => result.Succeeded().ShouldBe(true));
                                }));

                            Given("the application is running in the production environment.", () => settings.Stub(s => s.Environment).Return("Production").Repeat.Any()).Verify(() =>
                            {
                                Given("the application has an intitial user id for the admin user defined in configuration", () => settings.Stub(s => s.InitialAdminUserName).Return("NewAdminUserName")).Verify(() =>
                                    {
                                        Given("the username provided translates into the user id configured to be the initial admin user.", () => StubUserNameAndSlugPair("NewAdminUserName", "NewAdminUserSlug")).Verify(() =>
                                            Then("it should add the user to the admin role.", () => result.Subject.Roles.ShouldContain(Roles.Admin)));
    
                                        Given("the username provided does not translate into the user id configured to be the initial admin user.", () => StubUserNameAndSlugPair("RegularUser", "RegularUserSlug")).Verify(() =>
                                            AssertUserAddedToRegularUserRole());
                                    });
                                    

                                Given("the application does not have an initial user id for the admin user defined in configuration", () => settings.Stub(s => s.InitialAdminUserName).Return(null)).Verify(() => 
                                    AssertUserAddedToRegularUserRole());
                            });

                            Given("the application is not running in the production environment", () => settings.Stub(s => s.Environment).Return("Development").Repeat.Any()).Verify(() =>
                            {
                                Given("an admin user has not yet been created", () => SUT.Bus.Stub(b => b.Query(Arg<AdminUserExists>.Is.Anything)).Return(false)).Verify(() =>
                                        Then("it should add the user to the admin role.", () => result.Subject.Roles.ShouldContain(Roles.Admin)));
                            });

                            Given("the username provided is one of the prohibted values", () => SUT.Username = CreateAccount.ForbiddenUserSlugs[0]).Verify(() => 
                                Then("it should return an error indicating that the provided slug isn't available.", () => VerifyErrorResponse(EventSiteResources.CreateUser_UsernameNotAvailable))); 
                        });
                });
            });            
        }

        private void StubUserNameAndSlugPair(string username, string userSlug) {
            SUT.Username = username;
            SUT.SlugConverter.Stub(c => c.ToSlug(username)).Return(userSlug).Repeat.Any();
        }
        private void AssertUserAddedToRegularUserRole() {
            Then("it should add the new user to a single role.", () => result.Subject.Roles.Count.ShouldBe(1));
            Then("it should add the new user to the normal user role.", () => result.Subject.Roles.Single().ShouldBe(Roles.User)); 
        }

        private void VerifyErrorResponse(string expectedMessage) {
            result.Status.ShouldBe(ResultStatus.Failure);
            result.Message.ShouldBe(expectedMessage);
        }

        private void SetupMocks() {
            SUT.SecurityEncoder = Mock<ISecurityEncoder>();
            SUT.SlugConverter = Mock<ISlugConverter>();
            SUT.DocSession = Mock<IDocumentSession>();
            SUT.Bus = Mock<IApplicationBus>();
            SUT.State = Mock<IApplicationState>();
            //var settingsProvider = Mock<IAppSettingsProvider>();
            settings = Mock<ISettings>();
            SUT.State.Stub(s => s.Settings).Return(settings).Repeat.Any();
        }

        private void SetRequiredFields() {
            SUT.Username = "testUser";
            SUT.Email = "testUser@gmail.com";
            SUT.ExternalLoginData = ExternalLoginData;
        }
    }
}
