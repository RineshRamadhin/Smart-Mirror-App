using System;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    class Gestures : IMotionData
    {
        public string prop { get; set; }

        public Gestures()
        {
            this.prop = "TODO";
        }

        public bool UpdateData(Frame frame)
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {

                // Throw exception
                return false;
            }
        }
    }
}
