using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using Microsoft.Owin.Security.Google;
using Smart_Mirror_App_WPF.Authentication;
using System.Security.Claims;
using Owin;
using System.Diagnostics;

namespace Smart_Mirror_App_WPF.Authentication.Google
{
    /// <summary>
    /// Interaction logic for GoogleAuthentication.xaml
    /// </summary>
    public partial class AuthenticationPage : Page
    {
        public AuthenticationPage()
        {
            InitializeComponent();
            LoginGoogle();
        }

        private async void LoginGoogle()
        {
            AuthenticationGoogle googleAuthenticationService = new AuthenticationGoogle("user");
            await googleAuthenticationService.LoginGoogle();
        }
    }
}
