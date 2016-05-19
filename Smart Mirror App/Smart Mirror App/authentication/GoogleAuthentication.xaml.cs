using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Smart_Mirror_App.API.User;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Smart_Mirror_App.authentication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GoogleAuthentication : Page
    {
 
        UserOauthToken userToken = new UserOauthToken();
        Windows.Storage.ApplicationDataContainer sharedPreferences = Windows.Storage.ApplicationData.Current.LocalSettings;

        public GoogleAuthentication()
        {
            this.InitializeComponent();
            CheckCurrentAuthentication();
        }

        private void CheckCurrentAuthentication()
        {
            if (sharedPreferences.Values["googleToken"] != null)
            {
                NavigateToMainPage();
                //TODO: go to main page
            }
        }

        private void NavigateToMainPage()
        {
            Window.Current.Content = new MainPage();
        }

        private void SetGoogleToken(String TokenUri)
        {
            userToken.token = TokenUri;
            sharedPreferences.Values["googleToken"] = TokenUri;
        }

        public String GetOAuthToken()
        {
            return userToken.token;
        }

        private async void Login_Google_Plus(object sender, RoutedEventArgs e)
        {
            String googleClientId = "64494997844-hgvqajn97080vlv95tvnnjbkvtkt8tap.apps.googleusercontent.com";
            String googleCallbackUrl = "urn:ietf:wg:oauth:2.0:oob";
            String googleScope = "profile";

            try
            {
                String GoogleURL = "https://accounts.google.com/o/oauth2/auth?client_id=" + Uri.EscapeDataString(googleClientId) + "&redirect_uri=" + Uri.EscapeDataString(googleCallbackUrl) + "&response_type=code&scope=" + googleScope;

                Uri StartUri = new Uri(GoogleURL);
                // When using the desktop flow, the success code is displayed in the html title of this end uri
                Uri EndUri = new Uri("https://accounts.google.com/o/oauth2/approval?");

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, StartUri, EndUri);
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    SetGoogleToken(WebAuthenticationResult.ResponseData.ToString());
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    SetGoogleToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    SetGoogleToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception Error)
            {
                
            }
        }
    }
}
