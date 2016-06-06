using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GoogleCalendarService : DefaultGoogleService<GoogleCalendarModel, List<GoogleCalendarModel>, Events>
    {
        private List<GoogleCalendarModel> _gmails = new List<GoogleCalendarModel>();
        private UserCredential _credential;
        private string _applicationName = "Smart Mirror Google Calendar Service";

        public GoogleCalendarService(UserCredential credential)
        {
            this._credential = credential;
        }

        public override void CreateService()
        {
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = this._credential,
                ApplicationName = _applicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            this.ResponseParser(events);
        }

        protected override void ResponseParser(Events response)
        {
            
        }

        public override List<GoogleCalendarModel> GetData()
        {
            throw new NotImplementedException();
        }

        protected override void SetData(GoogleCalendarModel dataModel)
        {
            throw new NotImplementedException();
        }

        public override void InsertToDb(List<GoogleCalendarModel> data)
        {
            throw new NotImplementedException();
        }
    }
}
