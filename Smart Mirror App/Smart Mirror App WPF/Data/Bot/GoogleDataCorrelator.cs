using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;

namespace Smart_Mirror_App_WPF.Data.Bot
{
    public class GoogleDataCorrelator
    {
        public GoogleDataCorrelator()
        {

        }

        public string CorrelateCalendarWithGmail(List<GoogleCalendarModel> calenderEvents, List<GoogleGmailModel> mails)
        {
            return this.PredictEventCreatorMailedUser(calenderEvents, mails); 
        }

        private string PredictEventCreatorMailedUser(List<GoogleCalendarModel> calenderEvents, List<GoogleGmailModel> mails)
        {
            string possibleCorrelation = "";
            foreach (var mail in mails)
            {
                foreach (var calenderEvent in calenderEvents)
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
