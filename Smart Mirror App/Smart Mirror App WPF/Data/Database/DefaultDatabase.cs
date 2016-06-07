using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IDefaultDb
{
    SQLiteConnection CreateDb();
}

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class DefaultDatabase : IDefaultDb
    {
        private string _dbName = "usersdatabase.db";
        SQLiteConnection db;

        public SQLiteConnection CreateDb()
        {
            return db = new SQLiteConnection(_dbName);
        }
    }
}
