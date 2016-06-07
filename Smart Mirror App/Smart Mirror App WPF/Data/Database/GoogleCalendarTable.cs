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
            throw new NotImplementedException();
        }

        public override void InsertRow(GoogleCalendarModel model)
        {
            database.Insert(model);
        }

        protected override void UpdateRow(GoogleCalendarModel model)
        {
            throw new NotImplementedException();
        }
    }
}
