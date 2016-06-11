using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart_Mirror_App_WPF.Data.API;
using Smart_Mirror_App_WPF.Authentication.Google;
using Smart_Mirror_App_WPF.Data.Models;
using System.Threading.Tasks;
using Smart_Mirror_App_WPF.Data.Database;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smart_Mirror_App_WPF_Unit_Tests
{
    [TestClass]
    public class GoogleApiUnitTests
    {
        private AuthenticationGoogle googleAuthenticator = new AuthenticationGoogle();
        private GoogleProfileTable googleProfileDb = new GoogleProfileTable();

        private UserCredential user;
        private GoogleProfileModel userProfile;
        private GooglePlusService googlePlusService;
        private string _testMailId = "test";
        private string _testUsername = "user";

        [TestMethod]
        public async Task RequestGoogleUserProfile()
        {
            await this.SetupTestEnvironment();
            Assert.IsNotNull(userProfile.displayName);
        }

        [TestMethod]
        public async Task InsertedGoogleUserProfileInDb()
        {
             await this.SetupTestEnvironment();
             Assert.IsNotNull(googleProfileDb.GetRow(userProfile.smartMirrorUsername));
        }

        [TestMethod]
        public async Task RequestGoogleGmailData()
        {
            await googleAuthenticator.LoginGoogle(_testUsername);
            var gmailService = new GoogleGmailService(googleAuthenticator.GetCurrentCredentials());
            gmailService.CreateService();
            var mails = gmailService.GetData();
        }

        [TestMethod]
        public void InsertedGoogleGmailData()
        {
            var gmailTable = new GoogleGmailTable();
            var mail = new GoogleGmailModel
            {
                id = _testMailId
            };
            gmailTable.InsertRow(mail);
            Assert.IsNotNull(gmailTable.GetRow(_testMailId));
        }

        [TestMethod]
        public void UpdateGoogleMailRecord()
        {
            var gmailTable = new GoogleGmailTable();
            var mail = new GoogleGmailModel();
            mail.id = _testMailId;
            mail.subject = "testSubject";
            gmailTable.InsertRow(mail);
            Assert.IsNotNull(gmailTable.GetRow(_testMailId));
        }

        [TestMethod]
        public void DeleteGoogleGmailRecord()
        {
            var gmailTable = new GoogleGmailTable();
            gmailTable.DeleteRow(_testMailId);
            Assert.IsNull(gmailTable.GetRow(_testMailId));
        }

        [TestMethod]
        public void GetMailsOfUser()
        {
            var gmailTable = new GoogleGmailTable();
            var allMails = gmailTable.GetRecords(20, _testUsername);
            Assert.IsNotNull(allMails);
        }

        [TestMethod]
        public async Task RequestGoogleCalendarData()
        {
            await googleAuthenticator.LoginGoogle(_testUsername);
            var calendarService = new GoogleCalendarService(googleAuthenticator.GetCurrentCredentials());
            calendarService.CreateService();
        }

        [TestMethod]
        public void InsertGoogleCalendarData()
        {
            var calendarTable = new GoogleCalendarTable();
            var testEvent = new GoogleCalendarModel();
            testEvent.id = _testMailId;
            calendarTable.InsertRow(testEvent);
            Assert.IsNotNull(calendarTable.GetRow(_testMailId)); 
        }

        [TestMethod]
        public void UpdateGoogleCalendarEvent()
        {
            var calendarTable = new GoogleCalendarTable();
            var testEvent = new GoogleCalendarModel();
            testEvent.id = _testMailId;
            testEvent.summary = "Test Event WPF";
            calendarTable.InsertRow(testEvent);
            Assert.IsNotNull(calendarTable.GetRow(_testMailId).summary);
        }

        [TestMethod]
        public void DeleteGoogleCalendarEvent()
        {
            var calendarTable = new GoogleCalendarTable();
            calendarTable.DeleteRow(_testMailId);
            Assert.IsNull(calendarTable.GetRow(_testMailId));
        }

        [TestMethod]
        public void GetCalendarEventsUser()
        {
            var calendarTable = new GoogleCalendarTable();
            var allEvents = calendarTable.GetRecords(20, _testUsername);
            Assert.IsNotNull(allEvents);
        }

        [TestMethod]
        public async Task RequestUserProfileService()
        {
            await googleAuthenticator.LoginGoogle(_testUsername);
            var googlePlusService = new GooglePlusService(googleAuthenticator.GetCurrentCredentials());
            googlePlusService.CreateService();
            Assert.IsNotNull(googlePlusService.GetUserProfile());
        }

        [TestMethod]
        public void InsertedGoogleProfileInDb()
        {
            var googleProfileTable = new GoogleProfileTable();
            Assert.IsNotNull(googleProfileTable.GetRow(_testUsername));
        }

        [TestMethod]
        public async Task TestGoogleApiClient()
        {
            var googleAuthenticator = new AuthenticationGoogle();
            await googleAuthenticator.LoginGoogle(_testUsername);
            var googleApiClient = new GoogleApiClient(googleAuthenticator.GetCurrentCredentials());
            var user = googleApiClient.GetCurrentUser();
            Assert.IsNotNull(user);
            Assert.IsNotNull(googleApiClient.GetEventsUser());
            Assert.IsNotNull(googleApiClient.GetGmailsUser());
            Assert.IsNotNull(GoogleApiClient.GetCurrentWeather(user.location));
        }

        private async Task SetupTestEnvironment()
        {
            await googleAuthenticator.LoginGoogle(_testUsername);
            this.user = googleAuthenticator.GetCurrentCredentials();
            this.googlePlusService = new GooglePlusService(user);
            googlePlusService.CreateService();
            this.userProfile = googlePlusService.GetUserProfile();
        }
    }
}
