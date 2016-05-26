using System;
using System.Collections.Generic;
using System.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Smart_Mirror_App.Data.API.Google;
using Smart_Mirror_App.Data.Database;
using Smart_Mirror_App.Data.Models;

namespace Smart_Mirror_App.Authentication
{
    class AuthenticationGoogle
    {
        GoogleUserModel userToken = new GoogleUserModel();

        public async Task LoginGoogle()
        {
            ArrayList scopes = SetupScope();
            String GoogleURL = SetupGoogleAuthenticationUrl(scopes);

            try
            {
                Uri StartUri = new Uri(GoogleURL);
                // When using the desktop flow, the success code is displayed in the html title of this end uri
                Uri EndUri = new Uri("https://accounts.google.com/o/oauth2/approval?");

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, StartUri, EndUri);
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    string authenticationCode = ParseGoogleToken(WebAuthenticationResult.ResponseData.ToString());
                    JToken tokens = await RetrieveAuthenticationTokens(authenticationCode);
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    LogError("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    LogError("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
        }

        public GoogleUserModel GetUser()
        {
            return userToken;
        }

        private string ParseGoogleToken(string googleAuthenticationtToken)
        {
            if (string.IsNullOrEmpty(googleAuthenticationtToken)) return null;
            var parts = googleAuthenticationtToken.Split('=');
            for (int i = 0; i < parts.Length; ++i)
            {
                if (parts[i] == "Success code")
                {
                    return parts[i + 1];
                }
            }
            return null;
        }

        private async Task<JToken> RetrieveAuthenticationTokens(string googleAuthenticationToken)
        {
            var client = new HttpClient();
            var auth = await client.PostAsync("https://accounts.google.com/o/oauth2/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", googleAuthenticationToken),
                new KeyValuePair<string, string>("client_id",AuthenticationConstants.GoogleClientId),
                new KeyValuePair<string, string>("client_secret",AuthenticationConstants.GoogleClientSecret),
                new KeyValuePair<string, string>("grant_type","authorization_code"),
                new KeyValuePair<string, string>("redirect_uri","urn:ietf:wg:oauth:2.0:oob"),
            }));

            var data = await auth.Content.ReadAsStringAsync();
            Debug.WriteLine(data);
            var jToken = JToken.Parse(data);

            SetGoogleTokens(jToken);

            return jToken;
        }

        private void SetGoogleTokens(JToken tokens)
        {
            var accesToken = tokens.SelectToken("access_token");
            var refreshToken = tokens.SelectToken("refresh_token");
            var expireTime = tokens.SelectToken("expires_in");
            var expireDate = SetExpireDate(Int32.Parse(expireTime.ToString()));

            userToken.accesToken = accesToken.ToString();
            userToken.refreshToken = refreshToken.ToString();
            userToken.expireDate = expireDate;

            GetUserProfile();
        }


        private async void GetUserProfile()
        {
            GoogleUserData userProfile = new GoogleUserData(userToken.accesToken);
            await userProfile.RequestUserProfile();
            GoogleUserModel user = userProfile.GetUserProfile();
            userToken.name = user.name;
            userToken.avatarUrl = user.avatarUrl;

            InsertUserDB(userToken);
        }

        private void InsertUserDB(GoogleUserModel user)
        {
            // TODO: get username and mail
            AuthenticationDB authenticationDatabase = new AuthenticationDB();
            authenticationDatabase.createUserDatabase();
            authenticationDatabase.insertUser(userToken);

            ArrayList test = authenticationDatabase.getAllUser();
        }

        private ArrayList SetupScope()
        {
            ArrayList scopes = new ArrayList();
            scopes.Add("profile");
            scopes.Add("https://www.googleapis.com/auth/calendar");
            scopes.Add("https://mail.google.com/");

            return scopes;
        }

        private String SetupGoogleAuthenticationUrl(ArrayList scopes)
        {
            String googleClientId = "64494997844-hgvqajn97080vlv95tvnnjbkvtkt8tap.apps.googleusercontent.com";
            String googleCallbackUrl = "urn:ietf:wg:oauth:2.0:oob";

            String GoogleUrl = "https://accounts.google.com/o/oauth2/auth?client_id="
                    + Uri.EscapeDataString(googleClientId) + "&redirect_uri="
                    + Uri.EscapeDataString(googleCallbackUrl)
                    + "&response_type=code&scope=";

            foreach (String scope in scopes)
            {
                GoogleUrl = GoogleUrl + "+" + scope;
            }

            return GoogleUrl;
        }

        private DateTime SetExpireDate(int seconds)
        {
            DateTime expireDate = DateTime.Now;
            expireDate.AddSeconds(seconds);

            return expireDate;
        }

        private void LogError(string error)
        {

        }
    }
}
