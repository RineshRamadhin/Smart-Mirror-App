using System;
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

            Data = new LeapMotionData();
            Listener = new LeapMotionListener(Data);
            Controller = new LeapMotionController();
        }

        public bool Connect()
        {
            try
            {
                Controller.Connect(Listener);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool Disconnect()
        {
            try
            {
                Controller.Disconnect(Listener);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }  
    }
}
