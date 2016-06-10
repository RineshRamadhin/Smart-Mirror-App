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
 
        GoogleUserModel currentUser = new GoogleUserModel();
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

        private async void Login_Google_Plus(object sender, RoutedEventArgs e)
        {
            try
            {
                AuthenticationGoogle googleAuthenticationService = new AuthenticationGoogle();
                await googleAuthenticationService.LoginGoogle();
                currentUser = googleAuthenticationService.GetUser();
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
        }
    }
}
