namespace Smart_Mirror_App_WPF.Data.API
{
    /// <summary>
    /// Default methods for creating a service
    /// </summary>
    /// <typeparam name="T">The Model of the data you need</typeparam>
    /// <typeparam name="TU">A list of the model</typeparam>
    /// <typeparam name="TZ">The service response class of the Google API</typeparam>
    public abstract class DefaultGoogleService<T, TU, TZ>
    {
        /// <summary>
        /// Creates the API service of Google
        /// </summary>
        public abstract void CreateService();

        /// <summary>
        /// Parse the response of the service request
        /// </summary>
        /// <param name="response">Response made by the service request </param>
        /// <returns></returns>
        protected abstract T ResponseParser(TZ response);

        /// <summary>
        /// Get parsed service data
        /// </summary>
        /// <returns>A list of the data in a model</returns>
        public abstract TU GetData();

        /// <summary>
        /// Sets the parsed service data
        /// </summary>
        /// <param name="itemList">List of the parsed items in a model</param>
        protected abstract void SetData(TU itemList);

        /// <summary>
        /// Inserts the request data in the db
        /// </summary>
        /// <param name="data">List of the parsed items in a model</param>
        public abstract void InsertToDb(TU data);
    }
}
