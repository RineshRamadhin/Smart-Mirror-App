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
        private readonly AuthenticationGoogle _googleAuthenticator = new AuthenticationGoogle();
        private readonly GoogleProfileTable _googleProfileDb = new GoogleProfileTable();

        private UserCredential _user;
        private GoogleProfileModel _userProfile;
        private GooglePlusService _googlePlusService;
        private readonly string _testMailId = "test";
        private readonly string _testUsername = "user";

        [TestMethod]
        public async Task RequestGoogleUserProfile()
        {
            await SetupTestEnvironment();
            Assert.IsNotNull(_userProfile.displayName);
        }

        [TestMethod]
        public async Task InsertedGoogleUserProfileInDb()
        {
             await SetupTestEnvironment();
             Assert.IsNotNull(_googleProfileDb.GetRow(_userProfile.smartMirrorUsername));
        }

        [TestMethod]
        public async Task RequestGoogleGmailData()
        {
            await _googleAuthenticator.LoginGoogle(_testUsername);
            var gmailService = new GoogleGmailService(_googleAuthenticator.GetCurrentCredentials());
            gmailService.CreateService();
            Assert.IsNotNull(gmailService.GetData());
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
            var mail = new GoogleGmailModel
            {
                id = _testMailId,
                subject = "testSubject"
            };
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
            await _googleAuthenticator.LoginGoogle(_testUsername);
            var calendarService = new GoogleCalendarService(_googleAuthenticator.GetCurrentCredentials());
            calendarService.CreateService();
        }

        [TestMethod]
        public void InsertGoogleCalendarData()
        {
            var calendarTable = new GoogleCalendarTable();
            var testEvent = new GoogleCalendarModel {id = _testMailId};
            calendarTable.InsertRow(testEvent);
            Assert.IsNotNull(calendarTable.GetRow(_testMailId)); 
        }

        [TestMethod]
        public void UpdateGoogleCalendarEvent()
        {
            var calendarTable = new GoogleCalendarTable();
            var testEvent = new GoogleCalendarModel
            {
                id = _testMailId,
                summary = "Test Event WPF"
            };
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
            await _googleAuthenticator.LoginGoogle(_testUsername);
            var googlePlusService = new GooglePlusService(_googleAuthenticator.GetCurrentCredentials());
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
            await _googleAuthenticator.LoginGoogle(_testUsername);
            _user = _googleAuthenticator.GetCurrentCredentials();
            _googlePlusService = new GooglePlusService(_user);
            _googlePlusService.CreateService();
            _userProfile = _googlePlusService.GetUserProfile();
        }
    }
}
