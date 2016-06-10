using System;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion
{
    class LeapMotionController : Controller, IMotionController
    {
        public bool Connect(Listener listener)
        {
            try
            {
                AddListener(listener);
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        public bool Disconnect(Listener listener)
        {
            try
            {
                RemoveListener(listener);
                Dispose();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
