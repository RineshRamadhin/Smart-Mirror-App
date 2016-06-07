using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class GoogleCalendarTable : DefaultDatabaseTable<GoogleCalendarModel>
    {
        public GoogleCalendarTable()
        {
            this.CreateTable();
        }

        public override GoogleCalendarModel GetRow(string primaryKey)
        {
            var calendarEvent = from wantedEvent in database.Table<GoogleCalendarModel>()
                        where wantedEvent.id.Equals(primaryKey)
                        select wantedEvent;

            return calendarEvent.FirstOrDefault();
        }

        public override void InsertRow(GoogleCalendarModel model)
        {
            if (CheckIfRecordExist(model.id))
            {
                this.UpdateRow(model);
            } else
            {
                database.Insert(model);
            } 
        }

        protected override void UpdateRow(GoogleCalendarModel model)
        {
            var existingEvent = this.GetRow(model.id);

            if (existingEvent != null)
            {
                database.BeginTransaction();
                database.Update(model);
                database.Commit();
            }
        }

        private bool CheckIfRecordExist(string primaryKey)
        {
            var existingRecord = this.GetRow(primaryKey);
            if (existingRecord == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
