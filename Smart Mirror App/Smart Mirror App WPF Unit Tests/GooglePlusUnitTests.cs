using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart_Mirror_App_WPF.Data.API;
using Smart_Mirror_App_WPF.Authentication.Google;
using Smart_Mirror_App_WPF.Data.Models;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF_Unit_Tests
{
    [TestClass]
    public class GooglePlusUnitTests
    {
        [TestMethod]
        public async Task RequestGoogleUserProfile()
        {
            AuthenticationGoogle googleAuthenticator = new AuthenticationGoogle();
            await googleAuthenticator.LoginGoogle("user");
            GoogleUserModel user = googleAuthenticator.GetCurrentUser();
            GooglePlusData googlePlus = new GooglePlusData(user.accessToken);
            await googlePlus.RequestUserProfile();
            GoogleProfileModel userProfile = googlePlus.GetUserProfile();
            if (userProfile.displayName == null)
            {
                Assert.Fail();
            }
        }
    }
}
