using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion
{
    class LeapMotionListener : Listener
    {
        // TODO: overwrite default leapmotion listener functions

        public override void OnConnect(Controller arg0)
        {
            base.OnConnect(arg0);
        }

        public override void OnDisconnect(Controller arg0)
        {
            base.OnDisconnect(arg0);
        }

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}
