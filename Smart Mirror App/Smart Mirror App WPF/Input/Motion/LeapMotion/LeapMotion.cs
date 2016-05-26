using System.Diagnostics;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion
{
    class LeapMotion : IMotion
    {
        public LeapMotionListener Listener { get; set; }
        public LeapMotionController Controller { get; set; }
        public LeapMotionData Data { get; set; }

        public LeapMotion()
        {
            Debug.WriteLine("Initalizing leapmotion");

            Listener = new LeapMotionListener();
            Controller = new LeapMotionController();
            Data = new LeapMotionData();
        }   
    }
}
