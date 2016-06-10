using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Model
{
    abstract class BaseModel : PropertyChangedBase
    {
        public abstract TimeSpan Interval { get; }

        public abstract void Update();

        internal void TimerTick(object sender, object e)
        {
            Update();
        }
    }
}