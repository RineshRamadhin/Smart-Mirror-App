using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Smart_Mirror_App_WPF.Data.Models;

namespace Smart_Mirror_App_WPF.Data.Bot
{
    public class BotClient
    {
        private List<GoogleCalendarModel> _calenderEvents;
        private List<GoogleGmailModel> _mails;
        private GoogleProfileModel _profile;

        private static readonly object SyncLock = new object();
        private static BotClient _instance;
        private GoogleDataCorrelator _googleDataCorrelator;

        // Constructor is 'protected'
        protected BotClient()
        {
        }

        public static BotClient GetBotClientInstance()
        {
            if (_instance != null) return _instance;
            lock (SyncLock)
            {
                if (_instance == null)
                {
                    _instance = new BotClient();
                }
            }

            return _instance;
        }

        public string StartBotClient(List<GoogleCalendarModel> calenderEvents, List<GoogleGmailModel> mails, GoogleProfileModel profile)
        {
            _calenderEvents = calenderEvents;
            _mails = mails;
            _profile = profile;
            _googleDataCorrelator = new GoogleDataCorrelator(calenderEvents, mails, profile);
            return EntrySentence();
        }

        private static string EntrySentence()
        {
            if (DateTime.Now.Hour < 12 && DateTime.Now.Hour > 0)
                return "Good Morning!";
            else if (DateTime.Now.Hour > 12 && DateTime.Now.Hour < 18)
                return "Good afternoon!";
            else if (DateTime.Now.Hour > 18 && DateTime.Now.Hour < 0)
                return "Good evening!";
            else
                return "You should be sleeping!";
        }

        public string GetAdviceBasedOnGoogleInformation()
        {
            return _googleDataCorrelator.CorrelateCalendarWithGmail();
        }

        public string GetUserBirthday()
        {
            return _googleDataCorrelator.GetUserBirthday();
        }

        public string CheckUserHasEventsToday()
        {
            return _googleDataCorrelator.CheckEventsToday();
        }
    }
}
