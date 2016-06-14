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

        /// <summary>
        /// Initializes the LeapMotion object with the default controller, listener and data object
        /// </summary>
        public LeapMotion()
        {
            Data = new LeapMotionData();
            Listener = new LeapMotionListener(Data);
            Controller = new LeapMotionController();
        }

        /// <summary>
        /// Connect the default listener to the controller using the controller's Connect method
        /// </summary>
        /// <returns>true when successful, otherwise false</returns>
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

        /// <summary>
        /// Disconnect the default listener from the controller using the controller's Disconnect method
        /// </summary>
        /// <returns>true when successful, otherwise false</returns>
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
