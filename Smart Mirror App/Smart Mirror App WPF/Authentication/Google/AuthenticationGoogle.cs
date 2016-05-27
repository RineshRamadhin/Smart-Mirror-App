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

namespace Smart_Mirror_App_WPF.Authentication.Google
{
    public class AuthenticationGoogle
    {
        private GoogleUserModel currentUser = new GoogleUserModel();
        private string smartMirrorUser;

        public AuthenticationGoogle(string smartMirrorUsername)
        {
            this.smartMirrorUser = smartMirrorUsername;
        }

        public async Task LoginGoogle()
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { CalendarService.Scope.Calendar, GmailService.Scope.GmailReadonly, PlusService.Scope.PlusMe },
                        this.smartMirrorUser, CancellationToken.None, new FileDataStore("Test.Auth.Store"));
                };

                var Service = new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google API Sample",
                };

                this.SetCurrentUser(credential);
                Debug.WriteLine(credential);
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
        }

        private void SetCurrentUser(UserCredential credential)
        {
            this.currentUser.accesToken = credential.Token.AccessToken;
            this.currentUser.refreshToken = credential.Token.RefreshToken;
            this.currentUser.name = smartMirrorUser;

            double expireInSeconds = (double) credential.Token.ExpiresInSeconds;
            this.currentUser.expireDate = this.ConvertExpireData(expireInSeconds);
        }

        private DateTime ConvertExpireData(double seconds)
        {
            DateTime expireDate = DateTime.Now;
            return expireDate.AddSeconds(seconds);
        }
    }
}
