﻿using System;
using SQLite;

namespace Smart_Mirror_App_WPF.Data.Models
{
    public class GoogleUserModel
    {
        [PrimaryKey]
        public string name { get; set; }
        public string avatarUrl { get; set; }
        public string accesToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime expireDate { get; set; }
    }
}
