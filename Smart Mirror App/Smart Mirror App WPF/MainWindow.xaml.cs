using Smart_Mirror_App_WPF.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Xml;
using Google.Apis.Auth.OAuth2;
using static System.Net.Mime.MediaTypeNames;
using Smart_Mirror_App_WPF.Data.API;
using Google.Apis.Calendar.v3.Data;
using Smart_Mirror_App_WPF.Authentication.Google;
using Smart_Mirror_App_WPF.Data.Bot;
using Smart_Mirror_App_WPF.Data.Models;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data;
using Smart_Mirror_App_WPF.Loaders;
using System.Diagnostics;

namespace Smart_Mirror_App_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserCredential _curentUserCredential;
        private GoogleApiClient _googleApiClient;
        private string _defaultUrl = "http://www.ad.nl/home/rss.xml";
        private XmlLoader _xmlLoader = new XmlLoader();
        private LeapMotion leapMotion;

        delegate void MyDel(BitmapImage img);


        public MainWindow()
        {
            InitializeComponent();
            _curentUserCredential = GoogleSignin("user").Result;
            FillInUi();
            GetRssFeed();
            startclock();
            leapMotion = new LeapMotion {Data = {Gestures = new Gestures(null, null, null, OnCircle)}};
            leapMotion.Connect();

        }

        private async void OnCircle(bool clockwise)
        {
            if (clockwise)
                _curentUserCredential = await GoogleSignin("Tjarda");
            else
                _curentUserCredential = await GoogleSignin("Rinesh");
            
            FillInUi();
        }

        private async Task<UserCredential> GoogleSignin(string username)
        {
            var googleAuthenticator = new AuthenticationGoogle();
            await googleAuthenticator.LoginGoogle(username);
            var currentUserCredentials = googleAuthenticator.GetCurrentCredentials();
            _googleApiClient = new GoogleApiClient(currentUserCredentials);
            return currentUserCredentials;
        }

        private List<GoogleGmailModel> GetGmailGoogleData()
        {
            return _googleApiClient.GetGmailsUser();
        }

        private List<GoogleCalendarModel> GetGoogleCalendarData()
        {
            return _googleApiClient.GetEventsUser();
        }

        private GoogleProfileModel GetGoogleProfileData()
        {
            return _googleApiClient.GetCurrentUser();
        }

        private async void FillInUi()
        {
            var weatherModel = await _googleApiClient.GetCurrentWeather("Rotterdam");
            var userProfile = GetGoogleProfileData();
            var mails = GetGmailGoogleData();
            var events = GetGoogleCalendarData();

            InsertUserImage(userProfile.imageUrl);

            FillMailsData(mails);
            FillCalendarData(events);

            var botClient = BotClient.GetBotClientInstance();
            botClient.StartBotClient(events, mails, userProfile);
            this.Dispatcher.Invoke((Action)(() =>
            {
                weather.Text = weatherModel.temp.ToString() + " degrees";
                displayName.Text = userProfile.displayName;

                if (botClient.GetAdviceBasedOnGoogleInformation() != "")
                {
                    BotText.Text = botClient.GetAdviceBasedOnGoogleInformation();
                }
                else if (botClient.GetUserBirthday() != "")
                {
                    BotText.Text = botClient.GetUserBirthday();
                }
            }));
        }

        private void InsertUserImage(string imgUrl)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imgUrl, UriKind.Absolute);
                bitmap.EndInit();
                userImage.Source = bitmap;
            } catch (Exception error)
            {
                Debug.WriteLine(error);
            }
            
        }

        private void FillMailsData(IReadOnlyList<GoogleGmailModel> mails)
        { 
            string mailsText = "";
            for (int i = 0; i < 5; i++)
            {
                var mailSubject = mails[i].subject;
                if (mailSubject.Length > 40)
                {
                    mailsText += mailSubject.Substring(0, 40) + "..." + "\r\n";
                }
                else
                {
                    mailsText += mailSubject + "\r\n";
                }
            }
      
            this.Dispatcher.Invoke((Action)(() =>
            {
                mail1.Text = mailsText;
            }));
        }

        private void FillCalendarData(IReadOnlyList<GoogleCalendarModel> events)
        { 
            string eventsText = "";
            for (int i = 0; i < events.Count; i++)
            {
                var eventSummary = events[i].summary;
                var eventDay = events[i].startDate.Day;
                var eventMonth = events[i].startDate.Month;

                if (eventSummary.Length > 40)
                {
                    eventsText +=  eventDay + "-" + eventMonth + "  "+ eventSummary.Substring(0, 40) + "..." + "\r\n";
                }
                else
                {
                    eventsText +=  eventDay + "-" + eventMonth + "  " + eventSummary +  "\r\n";
                }
            }

            this.Dispatcher.Invoke((Action) (() =>
            {
                if (eventsText == "")
                {
                    UpcomingEventsLabel.Text = "No upcoming events!";
                    UpcomingEventsList.Text = "";
                }
                else
                {
                    UpcomingEventsList.Text = eventsText;
                }
            }));
        }


        public void GetRssFeed()
        {
            XmlDocument rssFeed = new XmlDocument();
            rssFeed.Load(_defaultUrl);
            UpdateUI(rssFeed);
        }

        private void UpdateUI(XmlDocument xml)
        {
            if (xml.DocumentElement != null)
            {
                XmlNodeList items = xml.DocumentElement.FirstChild.SelectNodes("item");
                int x = 0;
                string adText = "Ad Headlines: " + "\r\n" ;
                if (items != null)
                    foreach (XmlNode item in items)
                    {
                        string itemTitle = item["title"]?.InnerText;
                        adText += itemTitle + "\r\n";
                        x++;
                        if (x == 7)
                        {
                            break;
                        }
                    }

                AdTextBlock.Text = adText;
            }
           
        }

        private void startclock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        public async void showWeather()
        {
            var openWeatherService = new OpenWeatherService();
            await openWeatherService.HttpRequestData("Rotterdam");
            var currentWeather = openWeatherService.GetData();
            var temperature = currentWeather.temp;
        }

        private void tickevent(object sender, EventArgs e)
        {
   
            time.Text = DateTime.Now.ToString("H:mm");
            date.Text = DateTime.Today.ToString("M", CultureInfo.CreateSpecificCulture("nl-BE"));
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            SettingsWindow settingsWindow = System.Windows.Application.Current.Windows
                                          .OfType<SettingsWindow>()
                                          .FirstOrDefault();
                           
            if (settingsWindow == null)
            {
                settingsWindow = new SettingsWindow();
                settingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                settingsWindow.Top = this.Top + 50;
                settingsWindow.Left = this.Left + 40;
            }
            settingsWindow.Show();
        }

      
        private void Rss_Button_Click(object sender, RoutedEventArgs e)
        {
            RssReaderWindow rssReaderWindow = System.Windows.Application.Current.Windows
                                         .OfType<RssReaderWindow>()
                                         .FirstOrDefault();

            if (rssReaderWindow == null)
            {
                rssReaderWindow = new RssReaderWindow();
                rssReaderWindow.Owner = System.Windows.Application.Current.MainWindow;
                rssReaderWindow.Top = this.Top + this.ActualHeight - 325;
                rssReaderWindow.Left = this.Left + 40;
                rssReaderWindow.GetRssFeed();
            }
            rssReaderWindow.Show();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
