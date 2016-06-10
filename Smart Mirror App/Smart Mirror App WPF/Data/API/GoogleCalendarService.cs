using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Smart_Mirror_App_WPF.Data.Database;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GoogleCalendarService : DefaultGoogleService<GoogleCalendarModel, List<GoogleCalendarModel>, Event>
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

            try
            {
                Events events = this.SetupServiceRequest(service).Execute();
                var allEvents = new List<GoogleCalendarModel>();
                foreach (var item in events.Items) {
                    allEvents.Add(this.ResponseParser(item));
                }
                this.SetData(allEvents);
                this.InsertToDb(allEvents);
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

        public override List<GoogleCalendarModel> GetData()
        {
            return this._calendarEvents;
        }

        protected override void SetData(List<GoogleCalendarModel> dataModel)
        {
            this._calendarEvents = dataModel;
        }

        protected override GoogleCalendarModel ResponseParser(Event response)
        {
            var calenderItem = new GoogleCalendarModel();
            calenderItem.userId = _credential.UserId;
            calenderItem.id = response.Id;
            calenderItem.attendees = FilterEventAttendeesMail(response);
            calenderItem.htmlLink = response.HtmlLink;
            calenderItem.location = response.Location;
            calenderItem.startDate = (DateTime)response.Start.DateTime;
            calenderItem.summary = response.Summary;
            calenderItem.creatorName = response.Creator.DisplayName;
            calenderItem.creatorMail = response.Creator.Email;

            return calenderItem;
        }

        private string FilterEventAttendeesMail(Event response)
        {
            string attendeesMails = "";
            foreach (var attendee in response.Attendees)
            {
                attendeesMails += attendee.Email;
            }

            return attendeesMails;
        }

        public override void InsertToDb(List<GoogleCalendarModel> data)
        {
            foreach (var calendarEvent in data)
            {
                new GoogleCalendarTable().InsertRow(calendarEvent);
            }
        }
    }
}
