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
        private readonly string _testUsername = "user";

        [TestMethod]
        public async Task GoogleDataMailEventCorrelatorTest()
        {
            var googleAuthenticator = new AuthenticationGoogle();
            await googleAuthenticator.LoginGoogle(_testUsername);
            var googleApiClient = new GoogleApiClient(googleAuthenticator.GetCurrentCredentials());
            var googleCorrelatorBot = new GoogleDataCorrelator(googleApiClient.GetEventsUser(), googleApiClient.GetGmailsUser(), googleApiClient.GetCurrentUser());
            var correlation = googleCorrelatorBot.CorrelateCalendarWithGmail();
            Assert.IsNotNull(correlation);
        }

        [TestMethod]
        public async Task GetUserBirthdayTest()
        {
            var googleAuthenticator = new AuthenticationGoogle();
            await googleAuthenticator.LoginGoogle(_testUsername);
            var googleApiClient = new GoogleApiClient(googleAuthenticator.GetCurrentCredentials());
            var googleCorrelatorBot = new GoogleDataCorrelator(googleApiClient.GetEventsUser(), googleApiClient.GetGmailsUser(), googleApiClient.GetCurrentUser());
            var correlation = googleCorrelatorBot.GetUserBirthday();
            Assert.IsNotNull(correlation);
        }

        [TestMethod]
        public async Task StartBotClientTest()
        {
            var googleAuthenticator = new AuthenticationGoogle();
            await googleAuthenticator.LoginGoogle(_testUsername);
            var googleApiClient = new GoogleApiClient(googleAuthenticator.GetCurrentCredentials());
            var bot = BotClient.GetBotClientInstance();
            Assert.IsNotNull(bot.StartBotClient(googleApiClient.GetEventsUser(), googleApiClient.GetGmailsUser(),
                googleApiClient.GetCurrentUser()));
        }

        [TestMethod]
        public void GetGmailCalendarBotClientTest()
        {
            var bot = SetupBot().Result;
            var advice = bot.GetAdviceBasedOnGoogleInformation();
            Assert.IsNotNull(advice);
        }

        [TestMethod]
        public void GetUserBirthFromBot()
        {
            var bot = SetupBot().Result;
            Assert.IsNotNull(bot.GetUserBirthday());
        }

        [TestMethod]
        public void CheckIfThereAreEventTodayBot()
        {
            var bot = SetupBot().Result;
            var sentence = bot.CheckUserHasEventsToday();
            Assert.IsNotNull(sentence);
        }

        private async Task<BotClient> SetupBot()
        {
            var googleAuthenticator = new AuthenticationGoogle();
            await googleAuthenticator.LoginGoogle(_testUsername);
            var googleApiClient = new GoogleApiClient(googleAuthenticator.GetCurrentCredentials());
            var bot = BotClient.GetBotClientInstance();
            bot.StartBotClient(googleApiClient.GetEventsUser(), googleApiClient.GetGmailsUser(),
                googleApiClient.GetCurrentUser());
            return bot;
        }
    }
}
