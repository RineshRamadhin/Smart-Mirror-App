using Json;
using Smart_Mirror_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Controls;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Smart_Mirror_App_WPF.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        private string city;

        public BlankPage1()
        {
            this.InitializeComponent();
            RequestWeather();
        }
        public async void RequestWeather()
        {
            WeatherModel requestResponse = new WeatherModel();
            HttpClient httpClient = new HttpClient();

            var searchUrl = "http://api.openweathermap.org/data/2.5/weather?q=Rotterdam&APPID=44bb36849c892e284affdb12b216d70e";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(searchUrl);
                //(response);
                string jsonWeather = await response.Content.ReadAsStringAsync();
                JsonObject weatherObject = JsonObject.Parse(jsonWeather);
                string city = weatherObject.GetNamedString("name");
                requestResponse.city = city;
                this.city = city;

                Debug.WriteLine(response.Content);
            }
            catch (HttpRequestException httpError)
            {
                Debug.WriteLine(httpError);
            }
        }
        private void getWeather()
        {
            RequestWeather();
        }

    }
}