using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.Models
{
    public class OpenWeatherModel
    {
        public float temp { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public string mainWeather { get; set; }
        public string mainWeatherDescription { get; set; }
        public float windSpeed { get; set; }
        public float pressure { get; set; }
        public float humidity { get; set; }
        public float tempMin { get; set; }
        public float tempMax { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string city { get; set; }
    }
}
