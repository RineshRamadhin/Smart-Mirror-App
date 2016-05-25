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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GoogleAuthentication : Page
    {
 
        GoogleUserModel userToken = new GoogleUserModel();
        Windows.Storage.ApplicationDataContainer sharedPreferences = Windows.Storage.ApplicationData.Current.LocalSettings;

        public GoogleAuthentication()
        {
            this.InitializeComponent();
            CheckCurrentAuthentication();
        }

        private void CheckCurrentAuthentication()
        {
            // TODO: Validation valid token
            if (sharedPreferences.Values["googleToken"] != null)
            {
                Object token = sharedPreferences.Values["googleToken"];
               // NavigateToMainPage();
            }
        }

        private void NavigateToMainPage()
        {
            Window.Current.Content = new MainPage();
        }

        private void NotifyError(string error)
        {

        }

        private async void SetGoogleToken(JToken tokens)
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

            InsertUserDB(userToken);
        }

        private void InsertUserDB(GoogleUserModel tokens)
        {
            // TODO: get username and mail
            AuthenticationDB authenticationDatabase = new AuthenticationDB();
            authenticationDatabase.createUserDatabase();
            authenticationDatabase.insertUser(userToken);

            ArrayList test = authenticationDatabase.getAllUser();
        }

        private DateTime SetExpireDate(int seconds)
        {
            DateTime expireDate = DateTime.Now;
            expireDate.AddSeconds(seconds);

            return expireDate;
        }

        public String GetAccesToken()
        {
            if (userToken.accesToken != null)
            {
                return userToken.accesToken;
            } else
            {
                return "No Access Token Available";
            }
        }

        private string GetGoogleSuccessCode(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;
            var parts = data.Split('=');
            for (int i = 0; i < parts.Length; ++i)
            {
                if (parts[i] == "Success code")
                {
                    return parts[i + 1];
                }
            }
            return null;
        }

        public async Task<JToken> GetToken(string code)
        {
            var client = new HttpClient();
            var auth = await client.PostAsync("https://accounts.google.com/o/oauth2/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id",AuthenticationConstants.GoogleClientId),
                new KeyValuePair<string, string>("client_secret",AuthenticationConstants.GoogleClientSecret),
                new KeyValuePair<string, string>("grant_type","authorization_code"),
                new KeyValuePair<string, string>("redirect_uri","urn:ietf:wg:oauth:2.0:oob"),
            }));

            var data = await auth.Content.ReadAsStringAsync();
            Debug.WriteLine(data);
            var jToken = JToken.Parse(data);

            SetGoogleToken(jToken);
            
            return jToken;
        }

        public async void RefreshTokens()
        {
            JToken newTokens = await RequestNewTokens(userToken.refreshToken);
            SetGoogleToken(newTokens);
        }

        private async Task<JToken> RequestNewTokens(string refreshToken)
        {
            var client = new HttpClient();
            var auth = await client.PostAsync("https://accounts.google.com/o/oauth2/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("client_id",AuthenticationConstants.GoogleClientId),
                new KeyValuePair<string, string>("client_secret",AuthenticationConstants.GoogleClientSecret),
                new KeyValuePair<string, string>("grant_type","refresh_token"),
            }));

            var data = await auth.Content.ReadAsStringAsync();
            Debug.WriteLine(data);
            var jToken = JToken.Parse(data);
            
            return jToken;
        }

        private async void Login_Google_Plus(object sender, RoutedEventArgs e)
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
                    string authenticationCode = GetGoogleSuccessCode(WebAuthenticationResult.ResponseData.ToString());
                    JToken tokens = await GetToken(authenticationCode);
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    NotifyError("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    NotifyError("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception Error)
            {

            }
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

            foreach (String scope in scopes) {
                GoogleUrl = GoogleUrl + "+" + scope;
            }

            return GoogleUrl;
        }
    }
}
