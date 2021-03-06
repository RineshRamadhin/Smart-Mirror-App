﻿using Smart_Mirror_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Smart_Mirror_App_WPF.Controller
{
    static class TimerController
    {
        public static void RegisterModel(BaseModel model)
        {
            var timer = new DispatcherTimer();
            timer.Interval = model.Interval;
            timer.Tick += model.TimerTick;
            timer.Start();
        }
    }
}