using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion.Data
{
    class LeapMotionData : IMotionData
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
            try
            {
                this.Hands.UpdateData(frame);
                this.Fingers.UpdateData(frame);
                this.Tools.UpdateData(frame);
                this.Gestures.UpdateData(frame);

                return true;
            }
            catch (Exception e)
            {
                
                Debug.WriteLine("LeapMotionData.UpdateData: " + e);
                return false;
            }
        }
    
    }
}
