using System;
using System.Windows.Input;
using eOdvjetnik.Views;
using System.Diagnostics;
//using CommunityToolkit.Mvvm.Input;

namespace eOdvjetnik.ViewModel
{
	public class AppShellViewModel
	{
        public ICommand ButtonClickCommand{ get; set; }

        public AppShellViewModel()
		{
			ButtonClickCommand = new Command(OnButtonClick);
		}

		private async void OnButtonClick()
		{

			await Shell.Current.GoToAsync(nameof(Kalendar));
			Debug.WriteLine("KLIKNO");
		}
	}
}

