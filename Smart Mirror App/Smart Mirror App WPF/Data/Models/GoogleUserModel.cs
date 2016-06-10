using System;
using SQLite;

namespace Smart_Mirror_App_WPF.Data.Models
{
    public class GoogleUserModel
    {
        [PrimaryKey]
        public string name { get; set; }
        public string avatarUrl { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime expireDate { get; set; }
        public string uniqueGestureLeapMotion { get; set; }
    }
}
