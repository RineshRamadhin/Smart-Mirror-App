using Smart_Mirror_App_WPF.Data.Models;
using SQLite;
using System.Collections;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class UsersTable : DefaultDatabaseTable<GoogleUserModel>
    {
        SQLiteConnection database;
        public UsersTable()
        {
            this.CreateTable();
        }

        protected override void CreateTable()
        {
            DefaultDatabase databaseConn = new DefaultDatabase();
            database = databaseConn.CreateDb();
            database.CreateTable<GoogleUserModel>();
        }

        public override void DeleteRow(string username)
        {
            database.Delete<GoogleUserModel>(username);
        }
        public override void InsertRow(GoogleUserModel user)
        {
            GoogleUserModel wantedUser = this.GetRow(user.name);
            if (wantedUser == null)
            {
                database.Insert(user);
            }
            else
            {
                this.UpdateRow(user);
            }
        }

        protected override void UpdateRow(GoogleUserModel newUserCredentials)
        {
            var wantedUser = this.GetRow(newUserCredentials.name);

            if (wantedUser != null)
            {
                database.BeginTransaction();
                database.Update(newUserCredentials);
                database.Commit();
            }
        }

        public override GoogleUserModel GetRow(string username)
        {
            var user = from wantedUser in database.Table<GoogleUserModel>()
                       where wantedUser.name.Equals(username)
                       select wantedUser;

            return user.FirstOrDefault();
        }

        public ArrayList GetAllUsers()
        {
            var query = database.Table<GoogleUserModel>();
            ArrayList users = new ArrayList();

            foreach (var user in query)
            {
                GoogleUserModel gotUser = new GoogleUserModel();
                gotUser.name = user.name;
                gotUser.avatarUrl = user.avatarUrl;
                gotUser.accessToken = user.accessToken;
                gotUser.refreshToken = user.refreshToken;
                gotUser.expireDate = user.expireDate;

                users.Add(gotUser);
            }
            return users;
        }

        public void ClearUserDatabase(bool areYouSure)
        {
            if (areYouSure)
            {
                database.DeleteAll<GoogleUserModel>();
            }
        }
    }
}
