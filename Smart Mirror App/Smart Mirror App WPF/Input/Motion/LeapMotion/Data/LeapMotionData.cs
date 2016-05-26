using System;
using System.Diagnostics;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
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
