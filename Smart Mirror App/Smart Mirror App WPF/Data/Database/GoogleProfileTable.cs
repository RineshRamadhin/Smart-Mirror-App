using Smart_Mirror_App_WPF.Data.Models;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class GoogleProfileTable : DefaultDatabaseTable<GoogleProfileModel>
    {
        public GoogleProfileTable()
        {
            CreateTable();
        }
   
        /// <summary>
        /// Inserts the google user profile or updates it if user already exists
        /// </summary>
        /// <param name="profile">Google user profile from GooglePlusData</param>
        public override void InsertRow(GoogleProfileModel profile)
        {
            if (CheckIfProfileExist(profile))
                UpdateRow(profile);
            else
                Database.Insert(profile);
        }

        protected override void UpdateRow(GoogleProfileModel profile)
        {
            if (GetRow(profile.smartMirrorUsername) == null) return;
            Database.BeginTransaction();
            Database.Update(profile);
            Database.Commit();
        }

        public override GoogleProfileModel GetRow(string username)
        {
            return (from wantedProfile in Database.Table<GoogleProfileModel>()
                    where wantedProfile.smartMirrorUsername.Equals(username)
                    select wantedProfile).FirstOrDefault();
        }

        private bool CheckIfProfileExist(GoogleProfileModel profile)
        {
            return GetRow(profile.smartMirrorUsername) != null;
        }
    }
}
