﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion.Data
{
    class Hands : IMotionData
    {
        public string prop { get; set; }

        public Hands()
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
