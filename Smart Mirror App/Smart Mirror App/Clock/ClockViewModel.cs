using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App.Clock
{
    public class ClockViewModel : INotifyPropertyChanged
    {
        private string currentTime;
        private ClockModel model;
        public string CurrentTime
        {
            get
            {
                return currentTime;
            }
            set
            {
                if (currentTime != value)
                {
                    currentTime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        internal void Initialize(ClockModel model)
        {
            this.model = model;
            updateTime();
        }

        private void updateTime()
        {
            CurrentTime = $"it is {model.CurrentTime.ToString("hh:mm:ss tt")}.";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
