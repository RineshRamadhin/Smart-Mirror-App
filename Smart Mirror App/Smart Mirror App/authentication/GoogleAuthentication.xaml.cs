using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Authentication.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Smart_Mirror_App.authentication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GoogleAuthentication : Page
    {
        String googleApiToken = "";

        public GoogleAuthentication()
        {
            this.InitializeComponent();
        }

        public string GetToken()
        {
            return googleApiToken;
        }

        private void SetGoogleToken(String TokenUri)
        {
            googleApiToken = TokenUri;
        }

        private async void Login_Google_Plus(object sender, RoutedEventArgs e)
        {
            String GoogleClientId = "64494997844-hgvqajn97080vlv95tvnnjbkvtkt8tap.apps.googleusercontent.com";
            String GoogleCallbackUrl = "urn:ietf:wg:oauth:2.0:oob";

            try
            {
                String GoogleURL = "https://accounts.google.com/o/oauth2/auth?client_id=" + Uri.EscapeDataString(GoogleClientId) + "&redirect_uri=" + Uri.EscapeDataString(GoogleCallbackUrl) + "&response_type=code&scope=" + Uri.EscapeDataString("http://picasaweb.google.com/data");

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
