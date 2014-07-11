using EventSite.Domain;
using EventSite.Infrastructure;
using Rhino.Mocks;
using Shouldly;
using SpecEasy;

namespace EventSite.Tests.Infrastructure
{
    public class WebConfigSettingsSpecs : Spec<WebConfigSettings> {

        public void Test() {
            
            AuthenticationModes? result = null;
            When("Getting the currently configured authentication mode.", () => result = SUT.AuthenticationMode);

            Given("the current instance does not have a value specified for the AuthenticationMode setting", () => StubAuthenticationModeProperty(null)).Verify(
                () => Then("it should fall back to the default authentication mode.", () => result.ShouldBe(AuthenticationModes.Default)));

            Given("the current instance has the 'Fake' value specified for the AuthenticationMode setting", () => StubAuthenticationModeProperty("fake")).Verify(
                () => Then("it should return the 'fake' value.", () => result.ShouldBe(AuthenticationModes.Fake)));

            Given("the current instance has the 'Actual' value specified for the AuthenticationMode setting", () => StubAuthenticationModeProperty("actual")).Verify(
                () => Then("it should return the 'actual' value", () => result.ShouldBe(AuthenticationModes.Actual)));

            Given("the current instance is configured with a value, but the value doesn't match any known authentication modes", () => StubAuthenticationModeProperty("foobar")).Verify(
                () => Then("it should return the 'default' value as a fallback", () => result.ShouldBe(AuthenticationModes.Default)));

            Given("the current instance has a mixed case, but otherwise valid, value for the AuthenticationMode setting", () => StubAuthenticationModeProperty("fAkE")).Verify(
                () => Then("it should ignore the case differences and return the proper value.", () => result.ShouldBe(AuthenticationModes.Fake)));
        }

        private void StubAuthenticationModeProperty(string desiredValue) {
            const string propertyKey = "EventSite.AuthenticationMode";
            Get<IAppSettingsProvider>().Stub(p => p.GetPropertyValue(propertyKey)).Return(desiredValue);
        }
    }
}
