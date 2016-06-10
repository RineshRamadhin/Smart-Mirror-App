using System;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    public class Devices : IMotionData
    {
        public string Prop { get; set; }

        public Devices()
        {
            Prop = "TODO";
        }

        public bool UpdataData()
        {
            try
            {
                // TODO
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}