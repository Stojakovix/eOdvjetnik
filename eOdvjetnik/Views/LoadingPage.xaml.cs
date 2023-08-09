using CommunityToolkit.Mvvm.Messaging;
using eOdvjetnik.ViewModel;

namespace eOdvjetnik.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.Current.GoToAsync("//Kalendar");
    }
}