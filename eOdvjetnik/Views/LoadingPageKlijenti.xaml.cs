using System.Diagnostics;

namespace eOdvjetnik.Views;


public partial class LoadingPageKlijenti : ContentPage
{
    
    public LoadingPageKlijenti()
	{
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Debug.WriteLine("LoadingPage:Klijenti");
        OpenKlijenti();
    }

    public async void OpenKlijenti()
    {
       await Shell.Current.GoToAsync("///Klijenti");
    }
    
}