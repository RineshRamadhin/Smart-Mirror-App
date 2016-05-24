using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App.Clock
{
    public class ClockViewModel : PropertyChangedBase
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
            model.PropertyChanged += ModelPropertyChanged;
        }
     
        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           if(e.PropertyName == nameof(model.CurrentTime))
            {
                updateTime();
            }
        }

        private void updateTime()
        {
            CurrentTime = model.CurrentTime.ToString("H:mm");
        }
    }
}
