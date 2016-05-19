using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App.Clock
{
    class ClockModel
    {
        DateTime currentTime;
        public DateTime CurrentTime {get; set;}

        public void Update()
        {
            CurrentTime = DateTime.Now;
        }
    
    }
}
