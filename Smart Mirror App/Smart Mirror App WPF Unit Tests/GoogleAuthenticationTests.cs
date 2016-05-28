using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart_Mirror_App_WPF.Authentication.Google;
using Smart_Mirror_App_WPF.Data.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using Smart_Mirror_App_WPF.Data.Database;
using System.Collections;

namespace Smart_Mirror_App_WPF_Unit_Tests
{
    /// <summary>
    /// This unit test will test the OAuth2.0 authorization process in our application including saving users in our database.
    /// !!! Before you start running these test you must have logged in before in our application using the username: test-user
    /// </summary>
    [TestClass]
    public class GoogleAuthenticationTests
    {
        string testUsername = "user";

        [TestMethod]
        public void CanInstantiateAuthenticationGoogle()
        {
            AuthenticationGoogle googleAuthenticatorService = new AuthenticationGoogle();
        }

        [TestMethod]
        public async Task GoogleAuthentication()
        {
            AuthenticationGoogle googleAuthenticatorService = new AuthenticationGoogle();
            await googleAuthenticatorService.LoginGoogle(testUsername);
            GoogleUserModel user =  googleAuthenticatorService.GetCurrentUser();
            if (user == null)
            {
                Assert.Fail();
            }
        }

        public async Task DeleteUserFromApplication()
        {
            AuthenticationGoogle googleAuthenticatorService = new AuthenticationGoogle();
            await googleAuthenticatorService.LogoutGoogle(testUsername);
        }
    }
}
