using Smart_Mirror_App_WPF.Data.Models;
using SQLite;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class GoogleProfileTable
    {
        SQLiteConnection db;
        public GoogleProfileTable()
        {
            CreateGoogleProfileTable();
        }

        public void CreateGoogleProfileTable()
        {
            db = new SQLiteConnection("usersdatabase.db");
            db.CreateTable<GoogleProfileModel>();
        }

        public void InsertProfile(GoogleProfileModel profile)
        {
            if (CheckIfProfileExist(profile))
            {
                this.UpdateUserProfile(profile);
            } else
            {
                db.Insert(profile);
            }
        }

        public GoogleProfileModel GetSpecificUserProfile(string username)
        {
            var userProfile = from wantedProfile in db.Table<GoogleProfileModel>()
                              where wantedProfile.smartMirrorUsername.Equals(username)
                              select wantedProfile;

            return userProfile.FirstOrDefault();
        }

        private void UpdateUserProfile(GoogleProfileModel profile)
        {
            var existingProfile = this.GetSpecificUserProfile(profile.smartMirrorUsername);

            if (existingProfile != null)
            {
                db.BeginTransaction();
                db.Update(profile);
                db.Commit();
            }
        }

        private bool CheckIfProfileExist(GoogleProfileModel profile)
        {
            GoogleProfileModel existingProfile = this.GetSpecificUserProfile(profile.smartMirrorUsername);
            if (existingProfile == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
        

     }
}
