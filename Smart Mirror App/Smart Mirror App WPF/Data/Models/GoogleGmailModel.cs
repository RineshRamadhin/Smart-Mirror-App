using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.Models
{
    public class GoogleGmailModel
    {
        public string id { get; set; }
        public string from { get; set; }
        public string snippet { get; set; }
        public string subject { get; set; }
        public IList<string> labels { get; set; }
    }
}
