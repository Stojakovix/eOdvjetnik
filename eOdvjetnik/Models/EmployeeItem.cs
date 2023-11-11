using System;
using System.ComponentModel;

namespace eOdvjetnik.Model
{
    public class EmployeeItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string? EmployeeHWID { get; set; }
        public string? Initals { get; set; }
        public int Active { get; set; }
        public int Type { get; set; }
        public string HasLicence { get; set; }
        private string opis;
        public string Opis
        {
            get { return opis; }
            set
            {
                if (opis != value)
                {
                    opis = value;
                    OnPropertyChanged(nameof(Opis));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}