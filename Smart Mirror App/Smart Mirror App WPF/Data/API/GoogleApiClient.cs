using Google.Apis.Auth.OAuth2;
using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GoogleApiClient
    {
        private UserCredential _credential;

        public GoogleApiClient(UserCredential credential)
        {
            this._credential = credential;
        }

        public GoogleProfileModel GetCurrentUser()
        {
            var googlePlusService = new GooglePlusService(_credential);
            googlePlusService.CreateService();
            return googlePlusService.GetUserProfile();
        }

        public List<GoogleCalendarModel> GetEventsUser()
        {
            var googleCalendarService = new GoogleCalendarService(_credential);
            googleCalendarService.CreateService();
            return googleCalendarService.GetData();
        }

        public List<GoogleGmailModel> GetGmailsUser()
        {
            var googleGmailService = new GoogleGmailService(_credential);
            googleGmailService.CreateService();
            return googleGmailService.GetData();
        }
    }
}
