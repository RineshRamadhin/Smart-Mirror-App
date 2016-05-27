using System;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    class Fingers : IMotionData
    {
        public string prop { get; set; }

        public Fingers()
        {
            this.prop = "TODO";
        }

        public bool UpdateData(FingerList fingers)
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
