using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;

namespace Smart_Mirror_App_WPF.Data.Bot
{
    public class GoogleDataCorrelator
    {
        private readonly List<GoogleCalendarModel> _calenderEvents;
        private readonly List<GoogleGmailModel> _mails;
        private readonly GoogleProfileModel _profile;

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

        public string GetUserBirthday()
        {
            var today = DateTime.Today;
            var parsedBirthday = DateTime.Parse(_profile.birthday);
            int age = today.Year - parsedBirthday.Year;
            if (parsedBirthday > today.AddYears(-age))
                age--;

            return (parsedBirthday.AddYears(age + 1) - today).TotalDays + " days till your birthday! :D" ;
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
                            possibleCorrelation = calenderEvent.creatorName + " send you an e-mail possibly about " + calenderEvent.summary + " event";
                            break;
                }
            }
            return possibleCorrelation;
        }

        private static bool CheckMailDateBeforeEventStart(DateTime mailDate, DateTime eventDate)
        {
            return (mailDate < eventDate);
        }

        private static bool CheckMailDateAfterEventCreation(DateTime mailDate, DateTime eventCreateDate)
        {
            return (mailDate > eventCreateDate);
        }

        private static bool CheckMailFromEventCreator(string mailFromUser, string eventCreatorMail)
        {
            return (mailFromUser.Contains(eventCreatorMail));
        }
    }
}
