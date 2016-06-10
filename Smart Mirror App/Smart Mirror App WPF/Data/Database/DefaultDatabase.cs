using SQLite;

internal interface IDefaultDb
{
    SQLiteConnection CreateDb();
}

namespace Smart_Mirror_App_WPF.Data.Database
{
    public class DefaultDatabase : IDefaultDb
    {
        private readonly string _dbName = "usersdatabase.db";
        private SQLiteConnection _db;

        public SQLiteConnection CreateDb()
        {
            return _db = new SQLiteConnection(_dbName);
        }
    }
}
