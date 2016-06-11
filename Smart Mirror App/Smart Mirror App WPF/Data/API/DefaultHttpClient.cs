using Smart_Mirror_App_WPF.Data.API.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.API
{
    public abstract class DefaultHttpClient<T> : IDefaultHttpClient<T>
    {
        /// <summary>
        /// Makes a HttpRequest for the wanted data
        /// </summary>
        /// <param name="query">additional data</param>
        /// <returns></returns>
        public abstract Task HttpRequestData(string query);

        /// <summary>
        /// The http response parser
        /// </summary>
        /// <param name="response"></param>
        protected abstract void ResponseParser(HttpResponseMessage response);
        public abstract T GetData();
        protected abstract void SetData(T dataModel);
        protected abstract void InsertToDb(T data);
    }
}
