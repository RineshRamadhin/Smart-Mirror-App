using System;
using Smart_Mirror_App_WPF.Data.Models;
using SQLite;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class GoogleProfileTable : DefaultDatabaseTable<GoogleProfileModel>
    {
        SQLiteConnection database;
        public GoogleProfileTable()
        {
            this.CreateTable();
        }
        
        protected override void CreateTable()
        {
            DefaultDatabase databaseConn = new DefaultDatabase();
            database = databaseConn.CreateDb();
            database.CreateTable<GoogleProfileModel>();
        }

        /// <summary>
        /// Inserts the google user profile or updates it if user already exists
        /// </summary>
        /// <param name="profile">Google user profile from GooglePlusData</param>
        public override void InsertRow(GoogleProfileModel profile)
        {
            if (CheckIfProfileExist(profile))
            {
                this.UpdateRow(profile);
            }
            else
            {
                database.Insert(profile);
            }
        }

        public override void DeleteRow(string primaryKey)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateRow(GoogleProfileModel profile)
        {
            var existingProfile = this.GetRow(profile.smartMirrorUsername);

            if (existingProfile != null)
            {
                database.BeginTransaction();
                database.Update(profile);
                database.Commit();
            }
        }

        public override GoogleProfileModel GetRow(string username)
        {
            var userProfile = from wantedProfile in database.Table<GoogleProfileModel>()
                              where wantedProfile.smartMirrorUsername.Equals(username)
                              select wantedProfile;

            return userProfile.FirstOrDefault();
        }

        private bool CheckIfProfileExist(GoogleProfileModel profile)
        {
            GoogleProfileModel existingProfile = this.GetRow(profile.smartMirrorUsername);
            if (existingProfile == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
