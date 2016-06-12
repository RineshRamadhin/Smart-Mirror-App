using System.Collections;
using Smart_Mirror_App_WPF.Data.Models;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class UsersTable : DefaultDatabaseTable<GoogleUserModel>
    {

        public UsersTable()
        {
            CreateTable();
        }

        public override void InsertRow(GoogleUserModel user)
        {
            if (GetRow(user.name) == null)
                Database.Insert(user);
            else
                UpdateRow(user);
        }

        protected override void UpdateRow(GoogleUserModel newUserCredentials)
        {
            if (GetRow(newUserCredentials.name) == null) return;
            Database.BeginTransaction();
            Database.Update(newUserCredentials);
            Database.Commit();
        }

        public override GoogleUserModel GetRow(string username)
        {
            return (from wantedUser in Database.Table<GoogleUserModel>()
                    where wantedUser.name.Equals(username)
                    select wantedUser).FirstOrDefault();
        }

        public ArrayList GetAllUsers()
        {
            var query = Database.Table<GoogleUserModel>();
            var users = new ArrayList();

            foreach (var user in query)
            {
                users.Add(user);
            }
            return users;
        }

        public void ClearUserDatabase(bool areYouSure)
        {
            if (areYouSure)
                Database.DeleteAll<GoogleUserModel>();
        }
    }
}
