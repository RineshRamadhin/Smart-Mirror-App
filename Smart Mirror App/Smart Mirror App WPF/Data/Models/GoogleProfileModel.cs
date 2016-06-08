using SQLite;
using System.Collections.Generic;

namespace Smart_Mirror_App_WPF.Data.Models
{
    public class GoogleProfileModel : BaseGoogleModel
    {
        [PrimaryKey]
        public string displayName { get; set; }
        public string gender { get; set; }
        public string imageUrl { get; set; }
    }
}
