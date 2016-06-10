using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion
{
    interface IMotionController
    {
        bool Connect(Listener listener);
        bool Disconnect(Listener listener);
    }
}
