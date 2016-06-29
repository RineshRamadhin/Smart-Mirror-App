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
    public class GoogleCalendarService : DefaultGoogleService<GoogleCalendarModel, Event>
    {
        private List<GoogleCalendarModel> _calendarEvents = new List<GoogleCalendarModel>();
        private readonly UserCredential _credential;
        private readonly string _applicationName = "Smart Mirror Google Calendar Service";

        public GoogleCalendarService(UserCredential credential)
        {
            _credential = credential;
            CreateService();
        }

        protected sealed override void CreateService()
        {
            var service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = this._credential,
                ApplicationName = _applicationName,
            });

            StartSerivce(service);    
        }

        private void StartSerivce(CalendarService service)
        {
            try
            {
                var events = SetupServiceRequest(service).Execute();
                var allEvents = new List<GoogleCalendarModel>();
                foreach (var item in events.Items)
                {
                    allEvents.Add(ResponseParser(item));
                }
                _calendarEvents = allEvents;
                SetData(_calendarEvents);
                InsertToDb(_calendarEvents);
            }
            catch (Exception error)
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
            var calenderItem = new GoogleCalendarModel();
            calenderItem.userId = _credential.UserId;
            calenderItem.id = response.Id;
            calenderItem.attendees = FilterEventAttendeesMail(response);
            calenderItem.htmlLink = response.HtmlLink;
            calenderItem.location = response.Location;
            if (response.Start.DateTime != null) calenderItem.startDate = (DateTime)response.Start.DateTime;
            calenderItem.summary = response.Summary;
            if (response.Created != null) calenderItem.createDate = (DateTime)response.Created;
            calenderItem.creatorName = response.Creator.DisplayName;
            calenderItem.creatorMail = response.Creator.Email;

            return calenderItem;
        }

        /// <summary>
        /// Put all atttendees in one string
        /// </summary>
        /// <param name="response">The Event response from the request</param>
        /// <returns>One string with all attendees of the event seperator by a "-"</returns>
        private static string FilterEventAttendeesMail(Event response)
        {
            return response.Attendees != null ? response.Attendees.Aggregate("", (current, attendee) => current + (attendee.Email + "-")) : "";
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
