using Google.Apis.Auth.OAuth2;
using Smart_Mirror_App_WPF.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GoogleApiClient
    {
        private readonly UserCredential _credential;

        /// <summary>
        /// The Google Api Client for retrieving google data. Support the following API:
        /// - Google Calendar
        /// - Gmail
        /// - Google Plus (user profile)
        /// </summary>
        /// <param name="credential"></param>
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

        public static async Task<OpenWeatherModel> GetCurrentWeather(string userLocation)
        {
            var openWeatherService = new OpenWeatherService();
            await openWeatherService.HttpRequestData(userLocation);
            return openWeatherService.GetData();
        }
    }
}
