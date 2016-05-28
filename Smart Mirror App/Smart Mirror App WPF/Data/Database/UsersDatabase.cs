using Smart_Mirror_App_WPF.Data.Models;
using System.Collections;
using SQLite;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class UsersDatabase
    {
        SQLiteConnection userDb;

        public UsersDatabase()
        {
            CreateUserDatabase();
        }
        public void CreateUserDatabase()
        {
            userDb = new SQLiteConnection("usersdatabase.db");
            userDb.CreateTable<GoogleUserModel>();
        }

        public void InsertUser(GoogleUserModel user)
        {
            userDb.Insert(user);
        }

        public ArrayList GetAllUsers()
        {
            var query = userDb.Table<GoogleUserModel>();
            ArrayList users = new ArrayList();

            foreach (var user in query)
            {
                GoogleUserModel gotUser = new GoogleUserModel();
                gotUser.name = user.name;
                gotUser.avatarUrl = user.avatarUrl;
                gotUser.accesToken = user.accesToken;
                gotUser.refreshToken = user.refreshToken;
                gotUser.expireDate = user.expireDate;

                users.Add(gotUser);
            }
            return users;
        }

        public GoogleUserModel GetSpecificUser(string username)
        {
            var user = from wantedUser in userDb.Table<GoogleUserModel>()
                       where wantedUser.name.Equals(username)
                       select wantedUser;
   
            return user.FirstOrDefault();
        }

        private void UpdateUserRow(string username)
        {
            var query = userDb.Table<GoogleUserModel>().Where(user => user.name == username);
        }

        public void DeleteSpecificUser(string username)
        {
            userDb.Delete<GoogleUserModel>(username);
        }

        public void ClearUserDatabase(bool areYouSure)
        {
            if (areYouSure)
            {
                userDb.DeleteAll<GoogleUserModel>();
            }            
        }
    }
}
