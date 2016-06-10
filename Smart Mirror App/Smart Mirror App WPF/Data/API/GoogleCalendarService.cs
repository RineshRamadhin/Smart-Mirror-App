using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private readonly UserCredential _credential;
        private readonly string _applicationName = "Smart Mirror Google Calendar Service";

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
                var events = SetupServiceRequest(service).Execute();
                var allEvents = events.Items.Select(this.ResponseParser).ToList();
                this.SetData(allEvents);
                this.InsertToDb(allEvents);
            } catch (Exception error)
            {
                Debug.WriteLine(error);
            }       
        }

        private static EventsResource.ListRequest SetupServiceRequest(CalendarService service)
        {
            var request = service.Events.List("primary");
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
            var calenderItem = new GoogleCalendarModel
            {
                userId = _credential.UserId,
                id = response.Id,
                attendees = FilterEventAttendeesMail(response),
                htmlLink = response.HtmlLink,
                location = response.Location,
                startDate = (DateTime) response.Start.DateTime,
                summary = response.Summary,
                createDate = (DateTime) response.Created,
                creatorName = response.Creator.DisplayName,
                creatorMail = response.Creator.Email
            };

            return calenderItem;
        }

        private static string FilterEventAttendeesMail(Event response)
        {
            return response.Attendees.Aggregate("", (current, attendee) => current + (attendee.Email + "-"));
        }

        public override void InsertToDb(List<GoogleCalendarModel> data)
        {
            var calendarTable = new GoogleCalendarTable();
            foreach (var calendarEvent in data)
            {
                calendarTable.InsertRow(calendarEvent);
            }
        }
    }
}
