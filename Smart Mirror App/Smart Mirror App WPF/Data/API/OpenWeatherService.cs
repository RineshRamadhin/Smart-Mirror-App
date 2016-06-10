using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Smart_Mirror_App_WPF.Data.API
{
    internal class OpenWeatherConstants
    {
        public const string Token = "ca6815d653dbe8e9abec6a4f0ca09032";
        public const string Metric = "&units=metric";
    }

    public class OpenWeatherService : DefaultHttpClient<OpenWeatherModel>
    {
        private readonly string _openWeatherToken;
        private OpenWeatherModel _weather;

        public OpenWeatherService()
        {
            this._openWeatherToken = OpenWeatherConstants.Token;
        }


        public override OpenWeatherModel GetData()
        {
            return _weather;
        }


        public override async Task HttpRequestData(string city)
        {
            var httpClient = new HttpClient();
            var tokenUrl = "&APPID=" + this._openWeatherToken;
            var weatherRequestUrl = "http://api.openweathermap.org/data/2.5/weather?q="+ city + tokenUrl + OpenWeatherConstants.Metric;

            try
            {
                ResponseParser(await httpClient.GetAsync(weatherRequestUrl));
            }
            catch (HttpRequestException httpError)
            {
                Debug.WriteLine(httpError);
            }
        }

        protected override void InsertToDb(OpenWeatherModel data)
        {
            throw new NotImplementedException();
        }

        protected override async void ResponseParser(HttpResponseMessage response)
        {
            var openWeatherModel = new OpenWeatherModel();
            var weatherResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
            openWeatherModel.temp = FloatParser(weatherResponse["main"]["temp"].ToString());
            openWeatherModel.city = weatherResponse["name"].ToString();
            openWeatherModel.mainWeatherDescription = weatherResponse["weather"][0]["description"].ToString();
            openWeatherModel.mainWeather = weatherResponse["weather"][0]["main"].ToString();
            openWeatherModel.longitude = FloatParser(weatherResponse["coord"]["lon"].ToString());
            openWeatherModel.latitude = FloatParser(weatherResponse["coord"]["lat"].ToString());
            openWeatherModel.pressure = FloatParser(weatherResponse["main"]["pressure"].ToString());
            openWeatherModel.humidity = FloatParser(weatherResponse["main"]["humidity"].ToString());
            openWeatherModel.tempMin = FloatParser(weatherResponse["main"]["temp_min"].ToString());
            openWeatherModel.tempMax = FloatParser(weatherResponse["main"]["temp_max"].ToString());

            this.SetData(openWeatherModel);
        }

        private static float FloatParser(string value)
        {
            return float.Parse(value);
        }

        protected override void SetData(OpenWeatherModel dataModel)
        {
            this._weather = dataModel;
        }
    }
}
