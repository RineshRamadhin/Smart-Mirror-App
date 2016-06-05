﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart_Mirror_App_WPF.Data.API;
using Smart_Mirror_App_WPF.Authentication.Google;
using Smart_Mirror_App_WPF.Data.Models;
using System.Threading.Tasks;
using Smart_Mirror_App_WPF.Data.Database;
using Google.Apis.Auth.OAuth2;

namespace Smart_Mirror_App_WPF_Unit_Tests
{
    [TestClass]
    public class GooglePlusUnitTests
    {
        private AuthenticationGoogle googleAuthenticator = new AuthenticationGoogle();
        private GoogleProfileTable googleProfileDb = new GoogleProfileTable();

        private GoogleUserModel user;
        private GoogleProfileModel userProfile;
        private GooglePlusData googlePlus;

        [TestMethod]
        public async Task RequestGoogleUserProfile()
        {
            await this.SetupTestEnvironment();
            if (userProfile.displayName == null)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task InsertedGoogleUserProfileInDb()
        {
            await this.SetupTestEnvironment();
            GoogleProfileModel dbUserProfile = googleProfileDb.GetRow(userProfile.smartMirrorUsername);
            if (dbUserProfile.smartMirrorUsername == null)
            {
                Assert.Fail();
            }
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

        private async Task SetupTestEnvironment()
        {
            await googleAuthenticator.LoginGoogle("user");
            this.user = googleAuthenticator.GetCurrentUser();
            this.googlePlus = new GooglePlusData(user.accessToken, user.name);
            await googlePlus.HttpRequestData();
            this.userProfile = googlePlus.GetData();
        }
    }
}