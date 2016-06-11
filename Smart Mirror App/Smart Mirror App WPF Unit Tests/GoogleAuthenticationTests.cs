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
            var googleAuthenticatorService = new AuthenticationGoogle();
        }

        /// <summary>
        /// Test if we user request user credentials from Google OAuth2.0 sign in
        /// NOTE!!: The next tests only works when you have signed in once using the web
        /// Reason: Since Google only allows web authentication, we cant automatically sign in and require user input;
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GoogleAuthentication()
        {
            var googleAuthenticatorService = new AuthenticationGoogle();
            await googleAuthenticatorService.LoginGoogle(testUsername);
            var user =  googleAuthenticatorService.GetCurrentUser();
            testUser = user;
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void GetSpecificUser()
        {
            var user = AuthenticationGoogle.GetSpecificUser(testUsername);
            if (user == null)
            {
                Assert.IsNull(user);
            }
        }

        /// <summary>
        /// Checks if user is inserted to databas
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task InsertedUserToDb()
        {
            string wantedUserInDb = testUsername;
            var googleAuthenticatorService = new AuthenticationGoogle();
            var userTable = new UsersTable();

            await googleAuthenticatorService.LoginGoogle(wantedUserInDb);
            var user = userTable.GetRow(wantedUserInDb);
            
            Assert.AreEqual(user.name, wantedUserInDb);
        }

        [TestMethod]
        public void GetAllUsersFromDb()
        {
            var googleAuthenticatorService = new AuthenticationGoogle();
            var userTable = new UsersTable();
            var allUsers = userTable.GetAllUsers();
        }

        [TestMethod]
        public async Task UpdateSpecificUser()
        {
            var googleAuthenticatorService = new AuthenticationGoogle();
            var userTable = new UsersTable();
            await googleAuthenticatorService.LoginGoogle(testUsername);
            var user = googleAuthenticatorService.GetCurrentUser();
            userTable.InsertRow(user);
            var updatedUser = AuthenticationGoogle.GetSpecificUser(testUsername);

            Assert.AreNotEqual(user.expireDate, updatedUser.expireDate);
        }

        //[TestMethod]
        public void DeleteSpecificUserFromDb()
        {
            var userTable = new UsersTable();
            userTable.DeleteRow(testUsername);
            this.GetSpecificUser();
        }

        //TODO: Check if DB is cleared
        public void ClearUserDb()
        {
            var userTable = new UsersTable();
            userTable.ClearUserDatabase(true);
        }

        //[TestMethod]
        public async Task DeleteUserFromApplication()
        {
            var googleAuthenticatorService = new AuthenticationGoogle();
            await googleAuthenticatorService.LogoutGoogle(testUsername);
        }
    }
}
