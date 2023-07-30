using System;
using System.Windows.Input;

namespace eOdvjetnik.Services
{
	public class Navigacija
	{
		public ICommand PocetnaClick { get; set; }
        public ICommand KalendarClick { get; set; }
        public ICommand SpisiClick { get; set; } 
        public ICommand DokumentiClick { get; set; }
        public ICommand TarifaClick { get; set; }
        public ICommand KorisnickaPodrskaClick { get; set; }
        public ICommand KontaktiClick { get; set; }
        public ICommand PostavkeClick { get; set; }


        public Navigacija()
		{
			PocetnaClick = new Command(OnPocetnaClick);
			KalendarClick = new Command(OnKalendarClick);
            SpisiClick = new Command(OnSpisiClick);
            DokumentiClick = new Command(OnDokumentiClick);
            TarifaClick = new Command(OnTarifaClick);
            KorisnickaPodrskaClick = new Command(OnKorisnickaClick);
            KontaktiClick = new Command(OnKontaktiClick);
            PostavkeClick = new Command(OnPostavkeClick);



        }


        public void OnPocetnaClick() {
			Shell.Current.GoToAsync("///MainPage");
		}
        public void OnKalendarClick()
        {
            Shell.Current.GoToAsync("///Kalendar");
        }
        public void OnSpisiClick()
        {
            Shell.Current.GoToAsync("///Spisi");
        }
        public void OnDokumentiClick()
        {
            Shell.Current.GoToAsync("///Dokumenti");
        }
        public void OnTarifaClick()
        {
            Shell.Current.GoToAsync("///Naplata");
        }
        public void OnKorisnickaClick()
        {
            Shell.Current.GoToAsync("///");
        }
        public void OnKontaktiClick()
        {
            Shell.Current.GoToAsync("///Klijenti");
        }
        public void OnPostavkeClick()
        {
            Shell.Current.GoToAsync("///Postavke");
        }
    }
}

