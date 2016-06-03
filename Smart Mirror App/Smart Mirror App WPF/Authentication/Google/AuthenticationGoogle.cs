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
using Google.Apis.Auth.OAuth2.Flows;
using System.Net.Http;
using System.Collections.Generic;

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
            GoogleUserModel user = GetSpecificUser(smartMirrorUsername);
            if (user != null)
            {
                await RefreshAuthenticationTokens(user); 
            } else
            {
                await AuthorizeUsingWeb(smartMirrorUsername);
            }
        }

        private async Task AuthorizeUsingWeb(string smartMirrorUsername)
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

                GoogleUserModel newUser = this.ParseUserCredentials(credential, smartMirrorUsername);
                this.SetCurrentUser(newUser);
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
        }

        public async Task RefreshAuthenticationTokens(GoogleUserModel user)
        {
            try
            {
                var client = new HttpClient();
                var auth = await client.PostAsync("https://accounts.google.com/o/oauth2/token", new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("refresh_token", user.refreshToken),
                new KeyValuePair<string, string>("client_id",AuthenticationConstants.GoogleClientId),
                new KeyValuePair<string, string>("client_secret",AuthenticationConstants.GoogleClientSecret),
                new KeyValuePair<string, string>("grant_type","refresh_token"),
            }));
                var tokensJson = await auth.Content.ReadAsStringAsync();
                GoogleRefreshTokenModel refreshTokens = JsonConvert.DeserializeObject<GoogleRefreshTokenModel>(tokensJson);
                user = this.ParseRefreshTokens(refreshTokens, user);
                this.SetCurrentUser(user);
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
            DeleteSpecificUserFromDb(smartMirrorUsername);
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

        private GoogleUserModel ParseUserCredentials(UserCredential credential, string smartMirrorUsername)
        {
            GoogleUserModel user = new GoogleUserModel();

            user.accessToken = credential.Token.AccessToken;
            user.refreshToken = credential.Token.RefreshToken;
            user.name = smartMirrorUsername;

            double expireInSeconds = (double)credential.Token.ExpiresInSeconds;
            user.expireDate = this.ConvertExpireData(expireInSeconds);

            return user;
        }


        private GoogleUserModel ParseRefreshTokens(GoogleRefreshTokenModel refreshTokens, GoogleUserModel user)
        {
            user.accessToken = refreshTokens.access_token;
            double expireInSeconds = (double) refreshTokens.expires_in;
            user.expireDate = this.ConvertExpireData(expireInSeconds);

            return user;
        }

        private void SetCurrentUser(GoogleUserModel user)
        {
            this.currentUser = user;
            this.InsertUserInDb(this.currentUser);
        }

        private bool CheckUserExist(string smartMirrorUsername)
        {
            var user = this.GetSpecificUser(smartMirrorUsername);
            if (user == null)
            {
                return false;
            } else
            {
                return true;
            }
        }

        private void InsertUserInDb(GoogleUserModel user)
        {
            UsersTable userTable = new UsersTable();
            userTable.InsertRow(user);
        }

        public GoogleUserModel GetSpecificUser(string smartMirrorUsername)
        {
            UsersTable userTable = new UsersTable();
            GoogleUserModel specificUser = userTable.GetRow(smartMirrorUsername);

            return specificUser;
        }

        public void DeleteSpecificUserFromDb(string smartMirrorUsername)
        {
            UsersTable userTable = new UsersTable();
            userTable.DeleteRow(smartMirrorUsername);
        }

        private DateTime ConvertExpireData(double seconds)
        {
            DateTime expireDate = DateTime.Now;
            return expireDate.AddSeconds(seconds);
        }

    }
}
