using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;

namespace Smart_Mirror_App_WPF.Data.Bot
{
    public class GoogleDataCorrelator
    {
        private List<GoogleCalendarModel> _calenderEvents;
        private List<GoogleGmailModel> _mails;
        private GoogleProfileModel _profile;

        public GoogleDataCorrelator(List<GoogleCalendarModel> calenderEvents, List<GoogleGmailModel> mails, GoogleProfileModel profile)
        {
            this._calenderEvents = calenderEvents;
            this._mails = mails;
            this._profile = profile;
        }

        public string CorrelateCalendarWithGmail()
        {
            return this.PredictEventCreatorMailedUser(); 
        }

        private string PredictEventCreatorMailedUser()
        {
            string possibleCorrelation = "";
            foreach (var mail in _mails)
            {
                foreach (var calenderEvent in _calenderEvents)
                {
                    if (CheckMailFromEventCreator(mail.from, calenderEvent.creatorMail) 
                        && CheckMailDateAfterEventCreation(mail.date, calenderEvent.createDate) 
                        && CheckMailDateBeforeEventStart(mail.date, calenderEvent.startDate))
                    {
                        possibleCorrelation = calenderEvent.creatorName + " send you an e-mail possibly about " + calenderEvent.summary + " event";
                        break;
                    }
                }
            }
            return possibleCorrelation;
        }

        private bool CheckMailDateBeforeEventStart(DateTime mailDate, DateTime eventDate)
        {
            return (mailDate < eventDate);
        }

        private bool CheckMailDateAfterEventCreation(DateTime mailDate, DateTime eventCreateDate)
        {
            return (mailDate > eventCreateDate);
        }

        private bool CheckMailFromEventCreator(string mailFromUser, string eventCreatorMail)
        {
            return (mailFromUser.Contains(eventCreatorMail));
        }
    }
}
