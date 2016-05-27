using System;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    class Tools : IMotionData
    {
        public string prop { get; set; }

        public Tools()
        {
            this.prop = "TODO";
        }

        public bool UpdateData(ToolList tools)
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
