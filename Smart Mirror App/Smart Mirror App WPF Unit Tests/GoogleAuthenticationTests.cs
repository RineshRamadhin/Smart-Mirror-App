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
    /// !!!Before you start running these test you must have logged in before in our application using the username: test-user
    /// </summary>
    [TestClass]
    public class GoogleAuthenticationTests
    {
        string testUsername = "user";
        GoogleUserModel testUser = new GoogleUserModel();

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
            testUser = user;
            if (user == null)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSpecificUser()
        {
            AuthenticationGoogle googleAuthenticatorService = new AuthenticationGoogle();
            GoogleUserModel user = googleAuthenticatorService.GetSpecificUser(testUsername);
            if (user == null )
            {
                Assert.IsNull(user);
            }
        }

        [TestMethod]
        public async Task InsertedUserToDb()
        {
            string wantedUserInDb = testUsername;
            AuthenticationGoogle googleAuthenticatorService = new AuthenticationGoogle();
            UsersDatabase userDb = new UsersDatabase();
   
            await googleAuthenticatorService.LoginGoogle(wantedUserInDb);
            GoogleUserModel user = userDb.GetSpecificUser(wantedUserInDb);
            
            Assert.AreEqual(user.name, wantedUserInDb);
        }

        [TestMethod]
        public void GetAllUsersFromDb()
        {
            AuthenticationGoogle googleAuthenticatorService = new AuthenticationGoogle();
            UsersDatabase userDb = new UsersDatabase();
            ArrayList allUsers = userDb.GetAllUsers();
        }

        [TestMethod]
        public void DeleteSpecificUserFromDb()
        {
            UsersDatabase userDb = new UsersDatabase();
            userDb.DeleteSpecificUser(testUsername);
            this.GetSpecificUser();
        }

        public void ClearUserDb()
        {
            UsersDatabase userDb = new UsersDatabase();
            userDb.ClearUserDatabase(true);
        }

        public async Task DeleteUserFromApplication()
        {
            AuthenticationGoogle googleAuthenticatorService = new AuthenticationGoogle();
            await googleAuthenticatorService.LogoutGoogle(testUsername);
        }
    }
}
