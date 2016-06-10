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
                Database.Insert(user);
            else
                this.UpdateRow(user);
        }

        protected override void UpdateRow(GoogleUserModel newUserCredentials)
        {
            if (this.GetRow(newUserCredentials.name) == null) return;
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
