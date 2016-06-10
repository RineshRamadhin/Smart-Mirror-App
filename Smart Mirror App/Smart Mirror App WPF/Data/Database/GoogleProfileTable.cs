using System;
using Smart_Mirror_App_WPF.Data.Models;
using SQLite;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class GoogleProfileTable : DefaultDatabaseTable<GoogleProfileModel>
    {
        public GoogleProfileTable()
        {
            this.CreateTable();
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

        protected override void UpdateRow(GoogleProfileModel profile)
        {
            if (this.GetRow(profile.smartMirrorUsername) == null) return;
            database.BeginTransaction();
            database.Update(profile);
            database.Commit();
        }

        public override GoogleProfileModel GetRow(string username)
        {
            return (from wantedProfile in database.Table<GoogleProfileModel>()
                    where wantedProfile.smartMirrorUsername.Equals(username)
                    select wantedProfile).FirstOrDefault();
        }

        private bool CheckIfProfileExist(GoogleProfileModel profile)
        {
            return !(this.GetRow(profile.smartMirrorUsername) == null);
        }
    }
}
