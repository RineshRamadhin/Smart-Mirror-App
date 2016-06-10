using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart_Mirror_App_WPF.Authentication.Google;
using System.Threading.Tasks;
using Smart_Mirror_App_WPF.Data.API;
using Smart_Mirror_App_WPF.Data.Bot;

namespace Smart_Mirror_App_WPF_Unit_Tests
{
    [TestClass]
    public class BotUnitTests
    {
        private string _testUsername = "user";

        [TestMethod]
        public async Task GoogleDataMailEventCorrelatorTest()
        {
            var googleAuthenticator = new AuthenticationGoogle();
            await googleAuthenticator.LoginGoogle(_testUsername);
            var credentials = googleAuthenticator.GetCurrentCredentials();
            var googleApiClient = new GoogleApiClient(credentials);
            var googleCorrelatorBot = new GoogleDataCorrelator();
            var correlation = googleCorrelatorBot.CorrelateCalendarWithGmail(googleApiClient.GetEventsUser(), googleApiClient.GetGmailsUser());
            Assert.IsNotNull(correlation);
        }
    }
}
