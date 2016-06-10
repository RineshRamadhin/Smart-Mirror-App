using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace Smart_Mirror_App.InputControls.Motion
{
    interface IMotionData
    {
        bool UpdateData(Frame frame);
    }
}
