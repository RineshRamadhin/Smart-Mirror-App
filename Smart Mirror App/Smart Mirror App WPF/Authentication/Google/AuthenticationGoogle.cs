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
using System.Net.Http;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2.Flows;

namespace Smart_Mirror_App_WPF.Authentication.Google
{
    public class AuthenticationGoogle
    {
        private GoogleUserModel _currentUser;
        private UserCredential _currentUserCredential;
        private const string DataStoreLocation = "Test.Auth.Store";
        private const string GoogleClientSecretFileLocation = "client_secret.json";

        /// <summary>
        /// Starts OAuth2.0 web authorization against Google
        /// </summary>
        /// <param name="smartMirrorUsername">The custom username id to verify which user it is in our application</param>
        /// <returns></returns>
        public async Task LoginGoogle(string smartMirrorUsername)
        {
            await AuthorizeUsingWeb(smartMirrorUsername, GetGestureLeap());
        }

        /// <summary>
        /// Authorize using OAuth2.0 with Google API using Web
        /// User won't be redirected to the app so need to close the browser/tab.
        /// </summary>
        /// <param name="smartMirrorUsername"></param>
        /// <returns></returns>
        private async Task AuthorizeUsingWeb(string smartMirrorUsername, string gesture)
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream(GoogleClientSecretFileLocation, FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { CalendarService.Scope.Calendar, GmailService.Scope.GmailReadonly, GmailService.Scope.MailGoogleCom, PlusService.Scope.PlusMe },
                        smartMirrorUsername, CancellationToken.None, new FileDataStore(DataStoreLocation));
                };
                this.SetCurrentUser(this.ParseUserCredentials(credential, smartMirrorUsername, gesture));
                this._currentUserCredential = credential;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error);
            }
        }

        private static string GetGestureLeap()
        {
            return "";
        }

        /// <summary>
        /// NOT USED!! Since the Google API handles tokens refreshing automatically
        /// Request new tokens with a refresh token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
                GoogleRefreshTokenModel refreshTokens = JsonConvert.DeserializeObject<GoogleRefreshTokenModel>(await auth.Content.ReadAsStringAsync());
                this.SetCurrentUser(this.ParseRefreshTokens(refreshTokens, user));
            }
            catch (Exception error)
            {
                Debug.WriteLine(error);
            }
        }

        /// <summary>
        /// Deletes the user from our datastore
        /// </summary>
        /// <param name="smartMirrorUsername">The custom username id to verify which user it is in our application</param>
        /// <returns></returns>
        public async Task LogoutGoogle(string smartMirrorUsername)
        {
            _currentUser = new GoogleUserModel();
            var dataStore = new FileDataStore(DataStoreLocation);
            await dataStore.DeleteAsync<TokenResponse>(smartMirrorUsername);
            DeleteSpecificUserFromDb(smartMirrorUsername);
        }

        /// <summary>
        /// Switch the current user of the smart mirror to someone else if he/she exist in our application
        /// </summary>
        /// <param name="smartMirrorUsername">The custom username id to verify which user it is in our application</param>
        public async void SwitchGoogleUser(string smartMirrorUsername)
        {
            var dataStore = new FileDataStore(DataStoreLocation);
            var otherUser = await dataStore.GetAsync<TokenResponse>(smartMirrorUsername);
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
            return _currentUser;
        }

        /// <summary>
        /// Get the credentials of the user
        /// </summary>
        /// <returns></returns>
        public UserCredential GetCurrentCredentials()
        {
            return _currentUserCredential;
        }

        private GoogleUserModel ParseUserCredentials(UserCredential credential, string smartMirrorUsername, string gesture)
        {
            GoogleUserModel user = new GoogleUserModel();

            user.accessToken = credential.Token.AccessToken;
            user.refreshToken = credential.Token.RefreshToken;
            user.name = smartMirrorUsername;
            user.uniqueGestureLeapMotion = gesture;

            double expireInSeconds = (double)credential.Token.ExpiresInSeconds;
            user.expireDate = ConvertExpireData(expireInSeconds);

            return user;
        }


        private GoogleUserModel ParseRefreshTokens(GoogleRefreshTokenModel refreshTokens, GoogleUserModel user)
        {
            user.accessToken = refreshTokens.access_token;
            double expireInSeconds = (double) refreshTokens.expires_in;
            user.expireDate = ConvertExpireData(expireInSeconds);

            return user;
        }

        private void SetCurrentUser(GoogleUserModel user)
        {
            this._currentUser = user;
            InsertUserInDb(this._currentUser);
        }

        private bool CheckUserExist(string smartMirrorUsername)
        {
            return !(this.GetSpecificUser(smartMirrorUsername) == null);
        }

        private static void InsertUserInDb(GoogleUserModel user)
        {
            new UsersTable().InsertRow(user);
        }

        public GoogleUserModel GetSpecificUser(string smartMirrorUsername)
        {
            return new UsersTable().GetRow(smartMirrorUsername);
        }

        public void DeleteSpecificUserFromDb(string smartMirrorUsername)
        {
            new UsersTable().DeleteRow(smartMirrorUsername);
        }

        private static DateTime ConvertExpireData(double seconds)
        {
            return DateTime.Now.AddSeconds(seconds);
        }

    }
}
