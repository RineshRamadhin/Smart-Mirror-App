using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart_Mirror_App.Data.Models;
using System.Collections;

namespace Smart_Mirror_App.Data.Database
{
    class AuthenticationDB
    {

        string path;
        SQLite.Net.SQLiteConnection conn;

        public void createUserDatabase()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.
            Current.LocalFolder.Path, "db.sqlite");

            conn = new SQLite.Net.SQLiteConnection(new
               SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            conn.CreateTable<GoogleUserModel>();
        }

        public void insertUser(GoogleUserModel user)
        {
            conn.Insert(user);
        }

        public ArrayList getAllUser()
        {
            var query = conn.Table<GoogleUserModel>();
            ArrayList users = new ArrayList();

            foreach (var user in query)
            {
                GoogleUserModel gotUser = new GoogleUserModel();
                gotUser.id = user.id;
                gotUser.name = user.name;
                gotUser.avatarUrl = user.avatarUrl;
                gotUser.accesToken = user.accesToken;
                gotUser.refreshToken = user.refreshToken;
                gotUser.expireDate = user.expireDate;

                users.Add(gotUser);
            }
            return users;
        }

    }
}
