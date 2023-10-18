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
        IdiNaKalendar();
    }

    public async void IdiNaKalendar()
    {
       await Shell.Current.GoToAsync("///Klijenti");
    }
    
}