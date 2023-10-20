using System.Diagnostics;

namespace eOdvjetnik.Views;


public partial class LoadingPageNaplata : ContentPage
{
    
    public LoadingPageNaplata()
	{
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Debug.WriteLine("LoadingPage:Naplata");
        OpenNaplata();
    }

    public async void OpenNaplata()
    {
       await Shell.Current.GoToAsync("///Naplata");
    }
    
}