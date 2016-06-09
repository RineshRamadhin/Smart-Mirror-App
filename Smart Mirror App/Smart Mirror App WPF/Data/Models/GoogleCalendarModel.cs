using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.Models
{
    public class GoogleCalendarModel
    {
        [PrimaryKey]
        public string id { get; set; }
        public string attendees { get; set; }
        public string userId { get; set; }
        public string htmlLink { get; set; }
        public string summary { get; set; }
        public DateTime startDate { get; set; }
        public string location { get; set; }
        public string creatorName { get; set; }
        public string creatorMail { get; set; }
    }
}
