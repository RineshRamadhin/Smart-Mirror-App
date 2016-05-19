using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion.Data
{
    class LeapMotionData
    {
        public static Hands Hands { get; set; }
        public static Fingers Fingers { get; set; }
        public static Tools Tools { get; set; }
        public static Gestures Gestures { get; set; }

        public static void Init()
        {
            Hands = new Hands();
            Fingers = new Fingers();
            Tools = new Tools();
            Gestures = new Gestures();        
        }

        public static bool UpdateData(Frame frame)
        {
            // TODO: call update function in all child object with object specific data
            // child object throw exceptions on failure 

            // if one fails show exception
            return false;
        }
    
    }
}
