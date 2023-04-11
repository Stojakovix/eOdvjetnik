using System;
using System.ComponentModel;

namespace eOdvjetnik.Models
{
    public class Meeting : INotifyPropertyChanged
    {
        private string eventName;
        private DateTime from; 
        private DateTime to;
        private DateTime date;
        public string EventName
        {
            get { return eventName; }
            set
            {
                if (eventName != value)
                {
                    eventName = value;
                    OnPropertyChanged(nameof(EventName));
                }
            }
        }

        
        public DateTime From
        {
            get { return from; }
            set
            {
                if (from != value)
                {
                    from = value;
                    OnPropertyChanged(nameof(From));
                }
            }
        }

        
        public DateTime To
        {
            get { return to; }
            set
            {
                if (to != value)
                {
                    to = value;
                    OnPropertyChanged(nameof(To));
                }
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


}
