using EventSite.Domain.Commands;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client;
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
            When("creating a new account", () => result = SUT.Process());

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
                            Then("it should convert the external login information to a slug to be stored in the event site database.", () => SUT.SlugConverter.AssertWasCalled(c => c.ToSlug(Arg<string>.Is.Anything)));
                            //Then("it should check to see if the corresponding to the slu")
                   

                        });
                });
            });            
        }

        private void SetupMocks() {
            SUT.SecurityEncoder = Mock<ISecurityEncoder>();
            SUT.SlugConverter = Mock<ISlugConverter>();
            SUT.DocSession = Mock<IDocumentSession>();
        }

        private void SetRequiredFields() {
            SUT.Username = "testUser";
            SUT.Email = "testUser@gmail.com";
            SUT.ExternalLoginData = ExternalLoginData;
        }
    }
}
