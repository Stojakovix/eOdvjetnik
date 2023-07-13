using System;
using System.ComponentModel;
using System.Security.Policy;

namespace eOdvjetnik.Models
{
    public class ReceiptItem : INotifyPropertyChanged
    {

        public string Tbr { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }

        public string Points { get; set; }
        
        
        private bool ukupanIznosVisible { get; set; }

        public bool UkupanIznosVisible
        {
            get { return ukupanIznosVisible; }
            set
            {
                ukupanIznosVisible = value;
                OnPropertyChanged(nameof(UkupanIznosVisible));
            }
        }
        private int coefficient;
        public int Coefficient
        {
            get { return coefficient; }
            set
            {
                if (coefficient != value)
                {
                    coefficient = value;
                    OnPropertyChanged(nameof(Coefficient));
                    CoefficientChanged?.Invoke(this, EventArgs.Empty);
                    OnPropertyChanged(nameof(Amount));
                    OnPropertyChanged(nameof(Currency));
                    OnPropertyChanged(nameof(TotalAmount));
                    OnPropertyChanged(nameof(TotalAmountCurrency));

                }
            }
        }
        public ReceiptItem()
        {
            CoefficientChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(Amount));
                OnPropertyChanged(nameof(Currency));
            };
        }

        public event EventHandler CoefficientChanged;

        public float Amount
        {
            get
            {
                if (float.TryParse(Points, out float points))
                {
                    return points * 2f * Coefficient;
                }
                else { return 0f; }
            }

        }
        public string Currency
        {
            get { return $"{Amount.ToString("0.00")} €"; }
        }
        private float totalAmount;

        public float TotalAmount
        {
            get { return totalAmount; }
            set
            {
                if (totalAmount != value)
                {
                    totalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                    OnPropertyChanged(nameof(TotalAmountCurrency));
                }
            }
        }
        public string TotalAmountCurrency
        {
            get { return $"{TotalAmount.ToString("0.00")} €"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
