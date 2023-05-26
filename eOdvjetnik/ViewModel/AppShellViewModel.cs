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

        // popup komande
        public ICommand OnSupportClickCommand { get; set; }
        public ICommand PopupCloseCommand { get; set; }

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

        public string Version { get; set; }

        public AppShellViewModel()
		{
			MainClickCommand = new Command(OnMainClick);
			KalendarClickCommand = new Command(OnKalendarClick);
			DokumentiClickCommand = new Command(OnDokumentiClick);
            Version = $"Version {AppInfo.VersionString}";
            //Popup pozivanje
            PopupCloseCommand = new Command(PopupClose);
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

            await Shell.Current.GoToAsync("///Dokumenti");
            Debug.WriteLine("KLIKNO");
        }

        private async void OnKalendarClick()
        {

            await Shell.Current.GoToAsync(nameof(Kalendar) + "?cache=true");
            Debug.WriteLine("KLIKNO");
        }

        private void OnSupportClick()
        {
            PopupOpen = true;
            visible = true;
        }

        private void PopupClose()
        {
            PopupOpen = false;
            visible = false;
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

