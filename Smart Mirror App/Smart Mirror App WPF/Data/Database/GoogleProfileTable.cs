using Smart_Mirror_App_WPF.Data.Models;
using System.Collections;
using SQLite;

namespace Smart_Mirror_App_WPF.Data.Database
{
    class GoogleProfileTable
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
          if (profile.displayName != null)
            {
                db.Insert(profile);
            }
        }
    }
}
