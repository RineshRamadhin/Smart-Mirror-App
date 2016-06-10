using Smart_Mirror_App_WPF.Input.Motion.LeapMotion;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data;

namespace Smart_Mirror_App_WPF.Input.Motion
{
    interface IMotion
    {
        bool Connect();
        bool Disconnect();
    }
}
