using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart_Mirror_App_WPF.Data.API;
using Smart_Mirror_App_WPF.Authentication.Google;
using Smart_Mirror_App_WPF.Data.Models;
using System.Threading.Tasks;
using Smart_Mirror_App_WPF.Data.Database;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;

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
        string _testMailId = "test";

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
            GoogleProfileModel dbUserProfile = googleProfileDb.GetRow(userProfile.smartMirrorUsername);
            Assert.IsNotNull(dbUserProfile.smartMirrorUsername);
        }

        [TestMethod]
        public async Task RequestGoogleGmailData()
        {
            await googleAuthenticator.LoginGoogle("user");
            UserCredential credential = googleAuthenticator.GetCurrentCredentials();
            GoogleGmailService test = new GoogleGmailService(credential);
            test.CreateService();
            var mails = test.GetData();
        }

        [TestMethod]
        public void InsertedGoogleGmailData()
        {
            GoogleGmailTable gmailTable = new GoogleGmailTable();
            GoogleGmailModel mail = new GoogleGmailModel();
            mail.id = _testMailId;

            gmailTable.InsertRow(mail);
            var retrievedMail = gmailTable.GetRow(_testMailId);
            Assert.IsNotNull(retrievedMail);
        }

        [TestMethod]
        public void UpdateGoogleMailRecord()
        {
            GoogleGmailTable gmailTable = new GoogleGmailTable();
            GoogleGmailModel mail = new GoogleGmailModel();
            mail.id = _testMailId;
            mail.subject = "testSubject";
            gmailTable.InsertRow(mail);
            var retrievedMail = gmailTable.GetRow(_testMailId);
            Assert.IsNotNull(retrievedMail.subject);
        }

        [TestMethod]
        public void DeleteGoogleGmailRecord()
        {
            GoogleGmailTable gmailTable = new GoogleGmailTable();
            gmailTable.DeleteRow(_testMailId);
            var retrievedMail = gmailTable.GetRow(_testMailId);
            Assert.IsNull(retrievedMail);
        }

        [TestMethod]
        public async Task RequestGoogleCalendarData()
        {
            await googleAuthenticator.LoginGoogle("user");
            var credential = googleAuthenticator.GetCurrentCredentials();
            GoogleCalendarService calendarService = new GoogleCalendarService(credential);
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
        public async Task RequestUserProfileService()
        {
            await googleAuthenticator.LoginGoogle("user");
            var credential = googleAuthenticator.GetCurrentCredentials();
            var googlePlusService = new GooglePlusService(credential);
            googlePlusService.CreateService();
            var profile = googlePlusService.GetUserProfile();
            Assert.IsNotNull(profile);
        }

        [TestMethod]
        public void InsertedGoogleProfileInDb()
        {
            var googleProfileTable = new GoogleProfileTable();
            var profile = googleProfileTable.GetRow("user");
            Assert.IsNotNull(profile);
        }

        [TestMethod]
        public async Task TestGoogleApiClient()
        {
            await googleAuthenticator.LoginGoogle("user");
            var credentials = googleAuthenticator.GetCurrentCredentials();
            var googleApiClient = new GoogleApiClient(credentials);
            Assert.IsNotNull(googleApiClient.GetCurrentUser());
            Assert.IsNotNull(googleApiClient.GetEventsUser());
            Assert.IsNotNull(googleApiClient.GetGmailsUser());
        }


        private async Task SetupTestEnvironment()
        {
            await googleAuthenticator.LoginGoogle("user");
            this.user = googleAuthenticator.GetCurrentCredentials();
            this.googlePlusService = new GooglePlusService(user);
            googlePlusService.CreateService();
            this.userProfile = googlePlusService.GetUserProfile();
        }
    }
}
