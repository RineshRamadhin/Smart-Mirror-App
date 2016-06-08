using Smart_Mirror_App_WPF.Data.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.Database
{
    /// <summary>
    /// Interface for DB Tables
    /// </summary>
    /// <typeparam name="T">The model of the data you want to insert to db</typeparam>
    interface ITableFuctions<T> {
        void InsertRow(T model);
        void DeleteRow(string primaryKey);
        T GetRow(string primaryKey);
    }

    /// <summary>
    /// Default DB Table class
    /// </summary>
    /// <typeparam name="T">The model of the data you want to insert to db</typeparam>
    public abstract class DefaultDatabaseTable<T> : ITableFuctions<T>
    {
        protected SQLiteConnection database;
        /// <summary>
        /// Creates the table if it does not exist and makes a database connection
        /// </summary>
        protected void CreateTable() {
            DefaultDatabase databaseConn = new DefaultDatabase();
            database = databaseConn.CreateDb();
            database.CreateTable<T>();
        }

        /// <summary>
        /// Insert method of db
        /// </summary>
        /// <param name="model"></param>
        abstract public void InsertRow(T model);

        /// <summary>
        /// Delete record of the db
        /// </summary>
        /// <param name="primaryKey">Primary key of the record</param>
        public void DeleteRow(string primaryKey) {
            database.Delete<T>(primaryKey);
        }

        /// <summary>
        /// Updates a record that already exist in the table
        /// </summary>
        /// <param name="model"></param>
        abstract protected void UpdateRow(T model);

        /// <summary>
        /// Gets a record of the table
        /// </summary>
        /// <param name="primaryKey">Primary key of the record</param>
        /// <returns>The asked record</returns>
        abstract public T GetRow(string primaryKey);
    }
}
