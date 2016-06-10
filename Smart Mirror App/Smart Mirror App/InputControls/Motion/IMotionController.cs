using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using Smart_Mirror_App.InputControls.Motion.LeapMotion.Data;

namespace Smart_Mirror_App.InputControls.Motion
{
    interface IMotionController
    {
        bool Connect();
        bool Disconnect();
    }
}
