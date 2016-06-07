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
        private List<GoogleCalendarModel> _calendarEvents = new List<GoogleCalendarModel>();
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
            var calendarRequest = this.SetupServiceRequest(service);

            try
            {
                // List events.
                Events events = calendarRequest.Execute();
                this.ResponseParser(events);
            } catch (Exception error)
            {
                Debug.WriteLine(error);
            }
            
        }

        private EventsResource.ListRequest SetupServiceRequest(CalendarService service)
        {
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            return request;
        }

        protected override void ResponseParser(Events response)
        {
            foreach (var item in response.Items) {
                GoogleCalendarModel calenderItem = new GoogleCalendarModel();
                calenderItem.id = item.Id;
                calenderItem.htmlLink = item.HtmlLink;
                calenderItem.location = item.Location;
                calenderItem.startDate = (DateTime) item.Start.DateTime;
                calenderItem.summary = item.Summary;
                calenderItem.creatorName = item.Creator.DisplayName;
                calenderItem.creatorMail = item.Creator.Email;
                this.SetData(calenderItem);
            }
            
        }

        public override List<GoogleCalendarModel> GetData()
        {
            throw new NotImplementedException();
        }

        protected override void SetData(GoogleCalendarModel dataModel)
        {
            this._calendarEvents.Add(dataModel);
        }

        public override void InsertToDb(List<GoogleCalendarModel> data)
        {
            throw new NotImplementedException();
        }
    }
}
