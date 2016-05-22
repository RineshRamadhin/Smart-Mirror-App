using System;
using System.Collections.Generic;
using System.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Smart_Mirror_App.API.User;
using Smart_Mirror_App.Authentication;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Smart_Mirror_App.authentication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthenticationPage : Page
    {

        UserOauthToken userToken = new UserOauthToken();
        Windows.Storage.ApplicationDataContainer sharedPreferences = Windows.Storage.ApplicationData.Current.LocalSettings;

        public AuthenticationPage()
        {
            this.InitializeComponent();
            CheckCurrentAuthentication();
        }

        public String GetOAuthToken()
        {
            // TODO;
            return null;
        }

        private void NavigateToMainPage()
        {
            Window.Current.Content = new MainPage();
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

        private async void Login_Google_Plus(object sender, RoutedEventArgs e)
        {
            GoogleAuthentication googleLogin = new GoogleAuthentication();
            googleLogin.Login_Google_Plus(sender, e);
            UserOauthToken googleUser = googleLogin.GetGoogleLoginCredentials();
        }
    }
}
