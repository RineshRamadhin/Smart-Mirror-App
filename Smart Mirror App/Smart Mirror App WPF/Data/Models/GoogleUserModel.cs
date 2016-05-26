﻿using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App.Data.Models
{
    class GoogleUserModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string avatarUrl { get; set; }
        public string accesToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime expireDate { get; set; }
    }
}