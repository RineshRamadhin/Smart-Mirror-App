using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class GoogleGmailTable : DefaultDatabaseTable<GoogleGmailModel>
    {
        public GoogleGmailTable()
        {
            this.CreateTable();
        }

        public override GoogleGmailModel GetRow(string primaryKey)
        {
            var email = from wantedEmail in database.Table<GoogleGmailModel>()
                              where wantedEmail.id.Equals(primaryKey)
                              select wantedEmail;

            return email.FirstOrDefault();
        }

        public override void InsertRow(GoogleGmailModel model)
        {
            if (CheckRecordExist(model.id) && model.id != null)
            {
                this.UpdateRow(model);
            } else
            {
                this.database.Insert(model);
            }
            
        }

        public List<GoogleGmailModel> GetRecords(int some, string primaryKey)
        {
            var allMails = (from mails in database.Table<GoogleGmailModel>()
                                 where mails.userId.Equals(primaryKey)
                                 orderby mails.date descending
                                 select mails).Take(some); 

            return allMails.ToList();
        }


        protected override void UpdateRow(GoogleGmailModel model)
        {
            var existingMail = this.GetRow(model.id);

            if (existingMail != null)
            {
                database.BeginTransaction();
                database.Update(model);
                database.Commit();
            }
        }

        private bool CheckRecordExist(string primaryKey)
        {
            var mail = this.GetRow(primaryKey);
            if (mail == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
