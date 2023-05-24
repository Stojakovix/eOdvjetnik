using System;
using System.Windows.Input;
using eOdvjetnik.Views;
using System.Diagnostics;
using Syncfusion.Maui.Popup;
using System.ComponentModel;

namespace eOdvjetnik.ViewModel
{
	public class AppShellViewModel : INotifyPropertyChanged
	{
        public ICommand MainClickCommand { get; set; }
        public ICommand KalendarClickCommand { get; set; }
        public ICommand DokumentiClickCommand { get; set; }
        public ICommand OnSupportClickCommand { get; set; }

        private bool isOpen, visible;

        public bool PopupOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                OnPropertyChanged(nameof(PopupOpen));
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }

        public AppShellViewModel()
		{
			MainClickCommand = new Command(OnMainClick);
			KalendarClickCommand = new Command(OnKalendarClick);
			DokumentiClickCommand = new Command(OnDokumentiClick);
            OnSupportClickCommand = new Command(OnSupportClick);
           
            SfPopup popup = new SfPopup();
        }

		private async void OnMainClick()
		{

			await Shell.Current.GoToAsync("///MainPage");
			Debug.WriteLine("KLIKNO");
		}

        private async void OnDokumentiClick()
        {

            await Shell.Current.GoToAsync(nameof(Dokumenti));
            Debug.WriteLine("KLIKNO");
        }

        private async void OnKalendarClick()
        {

            await Shell.Current.GoToAsync(nameof(Kalendar));
            Debug.WriteLine("KLIKNO");
        }

        private void OnSupportClick()
        {
            PopupOpen = true;
            visible = true;
        }


        private async void ShowAlert(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
        // Mora bit kad god je INotifyPropertyChanged na pageu
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

