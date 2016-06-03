﻿using System.Net.Http;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.API
{
    interface IGoogleDataService<T>
    {
        T GetData();
    }

    public abstract class DefaultGoogleData<T> : IGoogleDataService<T>
    {
        public abstract Task HttpRequestData();
        protected abstract void ResponseParser(HttpResponseMessage response);
        public abstract T GetData();
        protected abstract void SetData(T dataModel);
        public abstract void InsertToDb(T data);
    }
}
