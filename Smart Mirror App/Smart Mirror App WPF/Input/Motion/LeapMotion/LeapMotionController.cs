using System;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion
{
    class LeapMotionController : Leap.Controller, IMotionController
    {
        /// <summary>
        /// Add's a given listener to this controller
        /// </summary>
        /// <param name="listener">listener</param>
        /// <returns>true when successful, otherwise false</returns>
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

        /// <summary>
        /// removes a given listener from this controller
        /// </summary>
        /// <param name="listener">listener</param>
        /// <returns>true when successful, otherwise false</returns>
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
