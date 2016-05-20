using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App.API.User
{
    public class UserOauthToken
    {
        public string accesToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime expireDate { get; set; }
    }
}
