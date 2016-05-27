using System;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    class Hands : IMotionData
    {
        public string prop { get; set; }

        public Hands()
        {
            this.prop = "TODO";
        }

        public bool UpdateData(HandList hands)
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
