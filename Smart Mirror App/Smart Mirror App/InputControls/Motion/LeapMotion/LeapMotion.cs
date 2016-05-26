using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using Smart_Mirror_App.InputControls.Motion.LeapMotion.Data;
using static System.Diagnostics.Debug;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion
{
    class LeapMotion : IMotion
    {
        public LeapMotionListener Listener { get; set; }
        public LeapMotionController Controller { get; set; }
        public LeapMotionData Data { get; set; }

        public LeapMotion()
        {
            WriteLine("Initalizing leapmotion");

            Listener = new LeapMotionListener();
            Controller = new LeapMotionController();
            Data = new LeapMotionData();
        }   
    }
}
