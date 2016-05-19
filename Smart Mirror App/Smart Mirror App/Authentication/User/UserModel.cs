using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App.API.User
{

    public class UserModel
    {
        public String userName { get; set; }
        public String userMail { get; set; }
        public String userImageUrl { get; set; }
    }

    public class UserOauthToken
    {
        public String token { get; set; }
    }

}
