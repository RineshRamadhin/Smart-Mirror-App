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
        public Hands Hands { get; set; }
        public Fingers Fingers { get; set; }
        public Tools Tools { get; set; }
        public Gestures Gestures { get; set; }

        public LeapMotionData()
        {
            this.Hands = new Hands();
            this.Fingers = new Fingers();
            this.Tools = new Tools();
            this.Gestures = new Gestures();
        }

        public bool UpdateData(Frame frame)
        {
            // TODO: call update function in all child object with object specific data
            // child object throw exceptions on failure
            this.Hands.UpdateData(frame);

            // if one fails show exception
            return false;
        }
    
    }
}
