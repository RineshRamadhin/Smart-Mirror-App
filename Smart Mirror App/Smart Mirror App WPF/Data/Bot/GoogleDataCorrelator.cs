using Smart_Mirror_App_WPF.Data.Models;
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
                    if (mail.from.Contains(calenderEvent.creatorMail) && mail.date < calenderEvent.startDate)
                    {
                        possibleCorrelation = calenderEvent.creatorName + " send you an e-mail possibly about " + calenderEvent.summary + " event";
                        break;
                    }
                }
            }
            return possibleCorrelation;
        }
    }
}
