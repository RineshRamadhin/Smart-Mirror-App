using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Model
{
    internal class WeatherModel
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string city { get; set; }
        public int temperature { get; set; }
    }
}
