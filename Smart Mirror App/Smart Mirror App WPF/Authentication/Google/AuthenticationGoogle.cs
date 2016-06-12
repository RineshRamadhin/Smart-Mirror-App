using System;
using System.IO;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Plus.v1;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;

using System.Diagnostics;
using Smart_Mirror_App_WPF.Data.Models;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Smart_Mirror_App_WPF.Data.Database;

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
            await AuthorizeUsingWeb(smartMirrorUsername, GetGestureLeap()).ConfigureAwait(false) ;
        }

        /// <summary>
        /// Authorize using OAuth2.0 with Google API using Web
        /// User won't be redirected to the app so need to close the browser/tab.
        /// </summary>
        /// <param name="smartMirrorUsername"></param>
        /// <param name="gesture"></param>
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
                }
                SetCurrentUser(ParseUserCredentials(credential, smartMirrorUsername, gesture));
                _currentUserCredential = credential;
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
        public async Task SwitchGoogleUser(string smartMirrorUsername)
        {
            var dataStore = new FileDataStore(DataStoreLocation);
            var otherUser = await dataStore.GetAsync<TokenResponse>(smartMirrorUsername);
            if (otherUser != null)
                await LoginGoogle(smartMirrorUsername).ConfigureAwait(false);
            else
                Debug.WriteLine(smartMirrorUsername + " does not exist in this application yet");
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

        private static GoogleUserModel ParseUserCredentials(UserCredential credential, string smartMirrorUsername, string gesture)
        {
            var user = new GoogleUserModel
            {
                accessToken = credential.Token.AccessToken,
                refreshToken = credential.Token.RefreshToken,
                name = smartMirrorUsername,
                uniqueGestureLeapMotion = gesture
            };

            if (credential.Token.ExpiresInSeconds == null) return user;
            double expireInSeconds = (double)credential.Token.ExpiresInSeconds;
            user.expireDate = ConvertExpireData(expireInSeconds);

            return user;
        }

        private void SetCurrentUser(GoogleUserModel user)
        {
            _currentUser = user;
            InsertUserInDb(_currentUser);
        }

        private static bool CheckUserExist(string smartMirrorUsername)
        {
            return GetSpecificUser(smartMirrorUsername) != null;
        }

        private static void InsertUserInDb(GoogleUserModel user)
        {
            new UsersTable().InsertRow(user);
        }

        public static GoogleUserModel GetSpecificUser(string smartMirrorUsername)
        {
            return new UsersTable().GetRow(smartMirrorUsername);
        }

        public static void DeleteSpecificUserFromDb(string smartMirrorUsername)
        {
            new UsersTable().DeleteRow(smartMirrorUsername);
        }

        private static DateTime ConvertExpireData(double seconds)
        {
            return DateTime.Now.AddSeconds(seconds);
        }

    }
}
