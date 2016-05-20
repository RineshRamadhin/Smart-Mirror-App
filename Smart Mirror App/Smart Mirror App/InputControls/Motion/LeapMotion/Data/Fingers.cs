using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion.Data
{
    class Fingers
    {
        public string prop { get; set; }

        public Fingers()
        {
            this.prop = "TODO";
        }

        public bool UpdateData(Frame frame)
        {
            // TODO: update data with object specific data
            // throw exceptions on failure
            this.prop = "updated value";

            // if one fails show exception
            return false;
        }
    }
}
