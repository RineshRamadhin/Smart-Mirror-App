using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.API.Models
{
    class GoogleProfileResponseModel
    {
        public string displayName { get; set; }
        public string gender { get; set; }
        public Dictionary<string, string> image { get; set; }
    }
}
