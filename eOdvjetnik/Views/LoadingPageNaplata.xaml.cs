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
        IdiNaKalendar();
    }

    public async void IdiNaKalendar()
    {
       await Shell.Current.GoToAsync("///Naplata");
    }
    
}