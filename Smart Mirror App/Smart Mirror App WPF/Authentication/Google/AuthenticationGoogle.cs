using System;
using System.IO;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using System.Diagnostics;
using Smart_Mirror_App_WPF.Data.Models;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;

namespace Smart_Mirror_App_WPF.Authentication.Google
{
    public class AuthenticationGoogle
    {
        private GoogleUserModel currentUser = new GoogleUserModel();
        private string smartMirrorUser;

        public async Task LoginGoogle(string user)
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { CalendarService.Scope.Calendar, GmailService.Scope.GmailReadonly, PlusService.Scope.PlusMe },
                        user, CancellationToken.None, new FileDataStore("Test.Auth.Store"));
                };

                var Service = new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google API Sample",
                };

                this.SetCurrentUser(credential);
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
        }

        public async Task LogoutGoogle(string user)
        {
            this.currentUser = new GoogleUserModel();
            FileDataStore dataStore = new FileDataStore("Test.Auth.Store");
            await dataStore.DeleteAsync<TokenResponse>(user);
            this.smartMirrorUser = "";
        }

        public async void SwitchGoogleUser(string user)
        {
            FileDataStore dataStore = new FileDataStore("Test.Auth.Store");
            TokenResponse otherUser = await dataStore.GetAsync<TokenResponse>(user);
            if (otherUser != null)
            {
                await LoginGoogle(user);
            }
            else
            {
                Debug.WriteLine(user + " does not exist in this application yet");
            }
        }

        private void SetCurrentUser(UserCredential credential)
        {
            this.currentUser.accesToken = credential.Token.AccessToken;
            this.currentUser.refreshToken = credential.Token.RefreshToken;
            this.currentUser.name = smartMirrorUser;

            double expireInSeconds = (double)credential.Token.ExpiresInSeconds;
            this.currentUser.expireDate = this.ConvertExpireData(expireInSeconds);
        }

        private DateTime ConvertExpireData(double seconds)
        {
            DateTime expireDate = DateTime.Now;
            return expireDate.AddSeconds(seconds);
        }
    }
}
