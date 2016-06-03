using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;

namespace Smart_Mirror_App_WPF.Data.API
{
    class GoogleCalendarData : DefaultGoogleData<GoogleCalendarModel>
    {

        private GoogleCalendarModel _calendar;
        private string _accessToken;

        public override GoogleCalendarModel GetData()
        {
            return _calendar;
        }

        public override async Task HttpRequestData()
        {

            HttpClient httpClient = new HttpClient();

            var requestUrl = "https://www.googleapis.com/plus/v1/people/me";
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                ResponseParser(response);
            }
            catch (HttpRequestException httpError)
            {
                Debug.WriteLine(httpError);
            }
        }

        public override void InsertToDb(GoogleCalendarModel data)
        {
            throw new NotImplementedException();
        }

        protected override void ResponseParser(HttpResponseMessage response)
        {
            throw new NotImplementedException();
        }

        protected override void SetData(GoogleCalendarModel dataModel)
        {
            throw new NotImplementedException();
        }
    }
}
