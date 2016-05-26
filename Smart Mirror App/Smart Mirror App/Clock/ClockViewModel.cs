using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart_Mirror_App.weather;
using System.Net.Http;
using System.Diagnostics;
using Windows.Data.Json;

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
        public string currentDate;
        public string CurrentDate
        {
            get
            {
                return currentDate;
            }
            set
            {
                if (currentDate != value)
                {
                    currentDate = value;
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
            CultureInfo dutch = new CultureInfo("nl-NL");
            CurrentDate = model.CurrentTime.ToString("d MMMM", dutch);
        }
    }
}
