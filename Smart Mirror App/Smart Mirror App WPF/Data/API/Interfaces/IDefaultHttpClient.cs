namespace Smart_Mirror_App_WPF.Data.API.Interfaces
{
    internal interface IDefaultHttpClient<out T>
    {
        T GetData();
    }
}