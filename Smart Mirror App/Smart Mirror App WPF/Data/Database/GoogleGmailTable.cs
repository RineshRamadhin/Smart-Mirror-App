﻿using Smart_Mirror_App_WPF.Data.Models;
using System.Collections.Generic;
using System.Linq;

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
            return (from wantedEmail in Database.Table<GoogleGmailModel>()
                    where wantedEmail.id.Equals(primaryKey)
                    select wantedEmail).FirstOrDefault();
        }

        public override void InsertRow(GoogleGmailModel model)
        {
            if (CheckRecordExist(model.id) && model.id != null)
                this.UpdateRow(model);
            else
                this.Database.Insert(model);
        }

        public List<GoogleGmailModel> GetRecords(int some, string primaryKey)
        {
            return (from mails in Database.Table<GoogleGmailModel>()
                    where mails.userId.Equals(primaryKey)
                    orderby mails.date descending
                    select mails).Take(some).ToList();
        }


        protected override void UpdateRow(GoogleGmailModel model)
        {
            if (this.GetRow(model.id) == null) return;
            Database.BeginTransaction();
            Database.Update(model);
            Database.Commit();
        }

        private bool CheckRecordExist(string primaryKey)
        {
            return !(this.GetRow(primaryKey) == null);
        }
    }
}
