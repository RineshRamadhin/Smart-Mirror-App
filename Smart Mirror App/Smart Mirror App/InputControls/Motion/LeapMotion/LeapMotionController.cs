using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using Smart_Mirror_App.InputControls.Motion.LeapMotion.Data;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion
{
    class LeapMotionController
    {
        private static Listener _listener;
        private static Controller _controller;
        private static LeapMotionData _data;

        public static void Init()
        {
            _controller = new Controller();
            _data = new LeapMotionData();
        }

        public static bool Connect()
        {
            // TODO: connect with leapmotion listener

            // on failure
            return false;
        }

        public static bool Disconnect()
        {
            // TODO: disconnect with the leapmotion listener

            // on failure
            return false;
        }

    }
}
