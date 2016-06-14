using Smart_Mirror_App_WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
using static System.Net.Mime.MediaTypeNames;
using Smart_Mirror_App_WPF.Data.API;
using Google.Apis.Calendar.v3.Data;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data;

namespace Smart_Mirror_App_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            startclock();

            LeapMotion leapMotion = new LeapMotion();
            leapMotion.Data.Gestures = new Gestures(null, null, null, OnCircle);
            leapMotion.Connect();
        }

        private void OnCircle(Boolean clockwise)
        {
            // Main Application implementation when circle gesture
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
            }
            rssReaderWindow.Show();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
