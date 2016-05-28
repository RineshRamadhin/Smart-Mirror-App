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
using Google.Apis.Json;
using Newtonsoft.Json;

using System.Diagnostics;
using Smart_Mirror_App_WPF.Data.Models;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Smart_Mirror_App_WPF.Data.Database;
using System.Collections;

namespace Smart_Mirror_App_WPF.Authentication.Google
{
    public class AuthenticationGoogle
    {
        private GoogleUserModel currentUser;
        private string dataStoreLocation = "Test.Auth.Store";
        private string googleClientSecretFileLocation = "client_secret.json";

        /// <summary>
        /// Starts OAuth2.0 web authorization against Google
        /// </summary>
        /// <param name="smartMirrorUsername">The custom username id to verify which user it is in our application</param>
        /// <returns></returns>
        public async Task LoginGoogle(string smartMirrorUsername)
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream(googleClientSecretFileLocation, FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { CalendarService.Scope.Calendar, GmailService.Scope.GmailReadonly, PlusService.Scope.PlusMe },
                        smartMirrorUsername, CancellationToken.None, new FileDataStore(this.dataStoreLocation));
                };

                var Service = new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google API Sample",
                };

                this.SetCurrentUser(credential, smartMirrorUsername);
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
        }

        /// <summary>
        /// Deletes the user from our datastore
        /// </summary>
        /// <param name="smartMirrorUsername">The custom username id to verify which user it is in our application</param>
        /// <returns></returns>
        public async Task LogoutGoogle(string smartMirrorUsername)
        {
            this.currentUser = new GoogleUserModel();
            FileDataStore dataStore = new FileDataStore(this.dataStoreLocation);
            await dataStore.DeleteAsync<TokenResponse>(smartMirrorUsername);
        }

        /// <summary>
        /// Switch the current user of the smart mirror to someone else if he/she exist in our application
        /// </summary>
        /// <param name="smartMirrorUsername">The custom username id to verify which user it is in our application</param>
        public async void SwitchGoogleUser(string smartMirrorUsername)
        {
            FileDataStore dataStore = new FileDataStore(this.dataStoreLocation);
            TokenResponse otherUser = await dataStore.GetAsync<TokenResponse>(smartMirrorUsername);
            if (otherUser != null)
            {
                await LoginGoogle(smartMirrorUsername);
            }
            else
            {
                Debug.WriteLine(smartMirrorUsername + " does not exist in this application yet");
            }
        }

        /// <summary>
        /// Get the current user
        /// </summary>
        /// <returns>The current user using GoogleUserModel</returns>
        public GoogleUserModel GetCurrentUser()
        {
            return currentUser;
        }

        private void SetCurrentUser(UserCredential credential, string smartMirrorUsername)
        {
            this.currentUser = new GoogleUserModel();

            this.currentUser.accesToken = credential.Token.AccessToken;
            this.currentUser.refreshToken = credential.Token.RefreshToken;
            this.currentUser.name = smartMirrorUsername;

            double expireInSeconds = (double)credential.Token.ExpiresInSeconds;
            this.currentUser.expireDate = this.ConvertExpireData(expireInSeconds);

        }

        private void InsertUserInDb(GoogleUserModel user)
        {
            UsersDatabase usersDb = new UsersDatabase();
            usersDb.InsertUser(user);
        }

        public GoogleUserModel GetSpecificUser(string smartMirrorUsername)
        {
            UsersDatabase usersDb = new UsersDatabase();
            GoogleUserModel specificUser = usersDb.GetSpecificUser(smartMirrorUsername);

            return specificUser;
        }

        private DateTime ConvertExpireData(double seconds)
        {
            DateTime expireDate = DateTime.Now;
            return expireDate.AddSeconds(seconds);
        }

    }
}
