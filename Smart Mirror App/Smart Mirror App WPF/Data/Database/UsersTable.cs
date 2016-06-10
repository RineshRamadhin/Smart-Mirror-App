using Smart_Mirror_App_WPF.Data.Models;
using System.Collections;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class UsersTable : DefaultDatabaseTable<GoogleUserModel>
    {

        public UsersTable()
        {
            this.CreateTable();
        }

        public override void InsertRow(GoogleUserModel user)
        {
            if (this.GetRow(user.name) == null)
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
            if (this.GetRow(newUserCredentials.name) == null) return;
            database.BeginTransaction();
            database.Update(newUserCredentials);
            database.Commit();
        }

        public override GoogleUserModel GetRow(string username)
        {
            return (from wantedUser in database.Table<GoogleUserModel>()
                    where wantedUser.name.Equals(username)
                    select wantedUser).FirstOrDefault();
        }

        public ArrayList GetAllUsers()
        {
            var query = database.Table<GoogleUserModel>();
            var users = new ArrayList();

            foreach (var user in query)
            {
                var gotUser = new GoogleUserModel
                {
                    name = user.name,
                    avatarUrl = user.avatarUrl,
                    accessToken = user.accessToken,
                    refreshToken = user.refreshToken,
                    expireDate = user.expireDate
                };

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
