using Smart_Mirror_App_WPF.Input.Motion.LeapMotion;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data;

namespace Smart_Mirror_App_WPF.Input.Motion
{
    interface IMotion
    {
        LeapMotionListener Listener
        {
            get;
            set;
        }

        LeapMotionController Controller
        {
            get;
            set;
        }

        LeapMotionData Data
        {
            get;
            set;
        }
    }
}
