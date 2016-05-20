using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using Smart_Mirror_App.InputControls.Motion.LeapMotion.Data;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion
{
    class LeapMotionController
    {
        public Listener Listener { get; set; }
        public Controller Controller { get; set; }
        public LeapMotionData Data { get; set; }

        public void Init()
        {
            Debug.WriteLine("Initalizing leapmotioncontroller");

            Listener = new LeapMotionListener();
            Controller = new Controller();
            Data = new LeapMotionData();
        }

        public bool Connect()
        {
            Controller.AddListener(Listener);

            // on failure
            return false;
        }

        public bool Disconnect()
        {
            Controller.RemoveListener(Listener);
            Controller.Dispose();

            // on failure
            return false;
        }

    }
}
