using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using Smart_Mirror_App.InputControls.Motion.LeapMotion.Data;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion
{
    class LeapMotionController : Controller
    {
        public bool Connect(Listener listener)
        {
            try
            {
                AddListener(listener);
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        public bool Disconnect(Listener listener)
        {
            try
            {
                RemoveListener(listener);
                Dispose();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}
