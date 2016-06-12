using System.Collections.Generic;
using System.Linq;
using Smart_Mirror_App_WPF.Data.Models;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class GoogleCalendarTable : DefaultDatabaseTable<GoogleCalendarModel>
    {
        public GoogleCalendarTable()
        {
            CreateTable();
        }

        public override GoogleCalendarModel GetRow(string primaryKey)
        {
            return (from wantedEvent in Database.Table<GoogleCalendarModel>()
                   where wantedEvent.id.Equals(primaryKey)
                   select wantedEvent).FirstOrDefault();
        }

        public override void InsertRow(GoogleCalendarModel model)
        {
            if (CheckIfRecordExist(model.id))
                UpdateRow(model);
            else
                Database.Insert(model);
        }

        public List<GoogleCalendarModel> GetRecords(int some, string primaryKey)
        {
            return (from events in Database.Table<GoogleCalendarModel>()
                    where events.userId.Equals(primaryKey)
                    orderby events.startDate descending
                    select events).Take(some).ToList();
        }

        protected override void UpdateRow(GoogleCalendarModel model)
        {
            if (GetRow(model.id) == null) return;
            Database.BeginTransaction();
            Database.Update(model);
            Database.Commit();
        }

        private bool CheckIfRecordExist(string primaryKey)
        {
            return GetRow(primaryKey) != null;
        }
    }
}
