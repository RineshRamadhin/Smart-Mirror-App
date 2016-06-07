using Smart_Mirror_App_WPF.Data.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.Database
{
    interface ITableFuctions<T> {
        void InsertRow(T model);
        void DeleteRow(string primaryKey);
        T GetRow(string primaryKey);
    }

    public abstract class DefaultDatabaseTable<T> : ITableFuctions<T>
    {
        protected SQLiteConnection database;
        protected void CreateTable() {
            DefaultDatabase databaseConn = new DefaultDatabase();
            database = databaseConn.CreateDb();
            database.CreateTable<T>();
        }
        
        abstract public void InsertRow(T model);
        public void DeleteRow(string primaryKey) {
            database.Delete<T>(primaryKey);
        }
        abstract protected void UpdateRow(T model);
        abstract public T GetRow(string primaryKey);
    }
}
