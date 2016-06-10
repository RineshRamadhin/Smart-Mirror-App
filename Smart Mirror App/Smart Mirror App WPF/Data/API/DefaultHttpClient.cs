using Smart_Mirror_App_WPF.Data.API.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.API
{
    public abstract class DefaultHttpClient<T> : IDefaultHttpClient<T>
    {
        public abstract Task HttpRequestData(string query);
        protected abstract void ResponseParser(HttpResponseMessage response);
        public abstract T GetData();
        protected abstract void SetData(T dataModel);
        protected abstract void InsertToDb(T data);
    }
}
