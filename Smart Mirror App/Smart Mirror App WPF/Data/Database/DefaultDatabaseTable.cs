using Smart_Mirror_App_WPF.Data.Database;
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
        abstract protected void CreateTable();
        abstract public void InsertRow(T model);
        abstract public void DeleteRow(string primaryKey);
        abstract protected void UpdateRow(T model);
        abstract public T GetRow(string primaryKey);
    }
}
